using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using GameStore.Web.App_LocalResources;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GameStore.Test.Web
{
    [TestClass]
    public class GlobalResTest
    {
        [TestMethod]
        public void Test_Get_All_Resources()
        {
            //Arrange
            var properties = typeof (GlobalRes).GetProperties().Where(p=>p.GetGetMethod().ReturnType==typeof(string));
            var values = new List<string>();

            //Act
            foreach (var property in properties)
            {
                values.Add(property.GetGetMethod().Invoke(null, null).ToString());
            }

            //Assert
            Assert.IsTrue(values.All(v=>!string.IsNullOrEmpty(v)));
        }

        [TestMethod]
        public void Test_Constructor_Resources()
        {
            //Arrange
            var contructor = typeof(GlobalRes).GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance,
                  null, Type.EmptyTypes, null);

            //Act
            var obj = contructor.Invoke(null);

            //Assert
            Assert.AreEqual(typeof (GlobalRes), obj.GetType());
        }

        [TestMethod]
        public void Test_set_culture()
        {
            //Arrange
            var culture = new CultureInfo("ru");
            //Act
            GlobalRes.Culture = culture;

            //Assert
            Assert.AreEqual(culture, GlobalRes.Culture);
        }
    }
}
