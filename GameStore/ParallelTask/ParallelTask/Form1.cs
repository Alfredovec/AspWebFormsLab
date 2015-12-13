using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ParallelTask
{
    public partial class Form1 : Form
    {
        private ConcurrentDictionary<int, Dictionary<char, long>> _dictionaries;
        
        private ConcurrentQueue<char> _queue;
        
        private readonly ReaderWriterLockSlim _locker = new ReaderWriterLockSlim();
        
        private readonly ManualResetEventSlim _resetEvent = new ManualResetEventSlim();
        
        private CancellationTokenSource _token;

        private bool _isEnd;

        public Form1()
        {
            InitializeComponent();
            button3.Enabled = false;
            button4.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pathLabel.Text = openFileDialog1.FileName;
            }
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(pathLabel.Text))
            {
                dataGridView1.Rows.Clear();
                _isEnd = false;
                _queue = new ConcurrentQueue<char>();
                _dictionaries = new ConcurrentDictionary<int, Dictionary<char, long>>();
                _token = new CancellationTokenSource();
                _resetEvent.Set();
                ChangeButtonStatus();
                
                var readTask = Task.Factory.StartNew(() => ReadFile(pathLabel.Text),_token.Token);
                for (int i = 0; i < Environment.ProcessorCount; i++)
                {
                    var threadIndex = i;
                    var workTask = Task.Factory.StartNew(() => AnalyzeText(threadIndex), _token.Token);
                }
                await Task.Factory.StartNew(MergeResult, _token.Token);
                ChangeButtonStatus();
            }
        }

        private void ChangeButtonStatus()
        {
            button1.Enabled = !button1.Enabled;
            button2.Enabled = !button2.Enabled;
            button3.Enabled = !button3.Enabled;
            button4.Enabled = !button4.Enabled;
        }

        private void MergeResult()
        {
            try
            {
                while (!_isEnd || _dictionaries.Any(d => d.Value.Count != 0))
                {
                    var merged = new Dictionary<char, long>();
                    _locker.EnterWriteLock();
                    foreach (var dictionary in _dictionaries)
                    {
                        foreach (var i in dictionary.Value)
                        {
                            if (merged.ContainsKey(i.Key))
                            {
                                merged[i.Key] += i.Value;
                            }
                            else
                            {
                                merged[i.Key] = i.Value;
                            }
                        }
                        dictionary.Value.Clear();
                    }
                    _locker.ExitWriteLock();
                    Invoke((Action) (() =>
                    {
                        foreach (var i in merged)
                        {
                            bool isFind = false;
                            for (int j = 0; j < dataGridView1.Rows.Count; j++)
                            {
                                if (dataGridView1[0, j].Value != null &&
                                    dataGridView1[0, j].Value.ToString()[0] == i.Key)
                                {
                                    dataGridView1[1, j].Value = Convert.ToInt64(dataGridView1[1, j].Value) + i.Value;
                                    isFind = true;
                                    break;
                                }
                            }
                            if (!isFind)
                            {
                                dataGridView1.Rows.Add(i.Key, i.Value);
                            }
                        }
                    }));
                    Thread.Sleep(100); //wait while other threads make some works.
                    _resetEvent.Wait();
                    _token.Token.ThrowIfCancellationRequested();
                }
            }
            catch
            {
                // ignored
            }
        }

        private void AnalyzeText(object o)
        {
            try
            {
                int index = (int) o;
                _dictionaries[index] = new Dictionary<char, long>();
                while (!_isEnd || _queue.Count != 0)
                {
                    char c;
                    if (!_queue.TryDequeue(out c))
                    {
                        continue;
                    }
                    _locker.EnterReadLock();
                    if (_dictionaries[index].ContainsKey(c))
                    {
                        _dictionaries[index][c]++;
                    }
                    else
                    {
                        _dictionaries[index][c] = 1;
                    }
                    _locker.ExitReadLock();
                    _resetEvent.Wait();
                    _token.Token.ThrowIfCancellationRequested();
                }
            }
            catch
            {
                // ignored
            }
        }

        private void ReadFile(string path)
        {
            try
            {
                using (var read = new StreamReader(path, Encoding.Unicode))
                {
                    while (read.Peek() >= 0)
                    {
                        char c = (char) read.Read();
                        _queue.Enqueue(c);
                        //Thread.Sleep(10); //make reading very slow
                        _resetEvent.Wait();
                        _token.Token.ThrowIfCancellationRequested();
                    }
                }
            }
            catch
            {
                // ignored
            }
            _isEnd = true;

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (_resetEvent.IsSet)
            {
                _resetEvent.Reset();
                button3.Text = "Возобновить";
            }
            else
            {
                _resetEvent.Set();
                button3.Text = "Пауза";
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (_token != null)
            {
                _token.Cancel();
                if (!_resetEvent.IsSet)
                {
                    _resetEvent.Set();
                }
                MessageBox.Show("Отменено");
            }
        }
    }
}
