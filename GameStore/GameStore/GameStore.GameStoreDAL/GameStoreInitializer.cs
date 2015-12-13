using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using GameStore.DALInfrastructure.RefModel;
using GameStore.GameStoreDAL.Model;
using GameStore.Models.Entities;
using GameStore.Models.Enums;

namespace GameStore.GameStoreDAL
{
    internal class GameStoreInitializer : DropCreateDatabaseAlways<GameStoreContext>
    {
        protected override void Seed(GameStoreContext context)
        {
            var roles = new[]
            {
                new Role {Name = "Administrator"},
                new Role {Name = "Manager"},
                new Role {Name = "Moderator"},
                new Role {Name = "User"},
            };
            foreach (var role in roles)
            {
                context.Roles.Add(role);
            }

            var users = new[]
            {
                new User {Name = "Admin", Email = "Stanislav_Zadorozhnii@epam.com", Password = "Password", Roles = new List<Role> {roles[0], roles[1]}},
                new User {Name = "Manager", Email = "stas249501@gmail.com", Password = "Password", Roles = new List<Role> {roles[1]}},
                new User {Name = "Moderator", Email = "moderator@email.com", Password = "Password", Roles = new List<Role> {roles[2]}},
                new User {Name = "Stas", Email = "user@email.com", Password = "Password", Roles = new List<Role> {roles[3]}},
                new User {Name = "Masha", Email = "user1@email.com", Password = "Password", Roles = new List<Role> {roles[3]}},
            };
            foreach (var user in users)
            {
                context.Users.Add(user);
            }

            var publishers = new[]
            {
                new Publisher {CompanyName = "Publ1", 
                    Translations = new List<PublisherTranslation>
                    {
                        new PublisherTranslation{Description = "Descrition publisher1", Language = Language.En},
                        new PublisherTranslation{Description = "Описание издателя1", Language = Language.Ru}
                    }, 
                    HomePage = "http://vk.com"
                },
                new Publisher
                {
                    CompanyName = "Publ2", 
                    Translations = new List<PublisherTranslation>
                    {
                        new PublisherTranslation{Description = "Descrition publisher2", Language = Language.En},
                        new PublisherTranslation{Description = "Описание издателя2", Language = Language.Ru}
                    }, 
                    HomePage = "http://epam.com"
                }
            };

            foreach (var publisher in publishers)
            {
                context.Publishers.Add(publisher);
            }

            var genres = new[]
            {
                new Genre
                {
                    Translations = new List<GenreTranslation>
                    {
                        new GenreTranslation{Language = Language.En, Name = "Strategy"},
                        new GenreTranslation{Language = Language.Ru, Name = "Стратегия"}
                    }
                },
                new Genre
                {
                    Translations = new List<GenreTranslation>
                    {
                        new GenreTranslation{Language = Language.En, Name = "RPG"},
                        new GenreTranslation{Language = Language.Ru, Name = "РПГ"}
                    }
                },
                new Genre
                {
                    Translations = new List<GenreTranslation>
                    {
                        new GenreTranslation{Language = Language.En, Name = "Sports"},
                        new GenreTranslation{Language = Language.Ru, Name = "Спорт"}
                    }
                },
                new Genre
                {
                    Translations = new List<GenreTranslation>
                    {
                        new GenreTranslation{Language = Language.En, Name = "Races"},
                        new GenreTranslation{Language = Language.Ru, Name = "Гонки"}
                    }
                },
                new Genre
                {
                    Translations = new List<GenreTranslation>
                    {
                        new GenreTranslation{Language = Language.En, Name = "Action"},
                        new GenreTranslation{Language = Language.Ru, Name = "Экшен"}
                    }
                },
                new Genre
                {
                    Translations = new List<GenreTranslation>
                    {
                        new GenreTranslation{Language = Language.En, Name = "Adventure"},
                        new GenreTranslation{Language = Language.Ru, Name = "Приключение"}
                    }
                },
                new Genre
                {
                    Translations = new List<GenreTranslation>
                    {
                        new GenreTranslation{Language = Language.En, Name = "Puzzle&Skill"},
                        new GenreTranslation{Language = Language.Ru, Name = "Головоломки"}
                    }
                },
                new Genre
                {
                    Translations = new List<GenreTranslation>
                    {
                        new GenreTranslation{Language = Language.Ru, Name = "Misc"}
                    }
                }
            };
            
            var genresChilds = new[]
            {
                new Genre
                {
                    Translations = new List<GenreTranslation>
                    {
                        new GenreTranslation{Language = Language.Ru, Name = "RTS"}
                    },
                    Parent = genres[0]
                },
                new Genre {
                    Translations = new List<GenreTranslation>
                    {
                        new GenreTranslation{Language = Language.Ru, Name = "TBS"}
                    }, 
                    Parent = genres[0]
                },
                new Genre
                {
                    Translations = new List<GenreTranslation>
                    {
                        new GenreTranslation{Language = Language.En, Name = "Rally"},
                        new GenreTranslation{Language = Language.Ru, Name = "Ралли"}
                    }, 
                    Parent = genres[3]
                },
                new Genre
                {
                    Translations = new List<GenreTranslation>
                    {
                        new GenreTranslation{Language = Language.En, Name = "Arcade"},
                        new GenreTranslation{Language = Language.Ru, Name = "Аркады"}
                    }, 
                    Parent = genres[3]
                },
                new Genre
                {
                    Translations = new List<GenreTranslation>
                    {
                        new GenreTranslation{Language = Language.En, Name = "Formula"},
                        new GenreTranslation{Language = Language.Ru, Name = "Формула"}
                    }, 
                    Parent = genres[3]
                },
                new Genre
                {
                    Translations = new List<GenreTranslation>
                    {
                        new GenreTranslation{Language = Language.Ru, Name = "Off-road"}
                    }, 
                    Parent = genres[3]
                },
                new Genre
                {
                    Translations = new List<GenreTranslation>
                    {
                        new GenreTranslation{Language = Language.Ru, Name = "FPS"}
                    },
                    Parent = genres[4]
                },
                new Genre
                {
                    Translations = new List<GenreTranslation>
                    {
                        new GenreTranslation{Language = Language.Ru, Name = "TPS"}
                    },
                    Parent = genres[4]
                }
            };
            
            foreach (var genresChild in genresChilds)
            {
                context.Genres.Add(genresChild);
            }

            var platformTypes = new[]
            {
                new PlatformType {Type = "Mobile"},
                new PlatformType {Type = "Browser"},
                new PlatformType {Type = "Desktop"},
                new PlatformType {Type = "Console"}
            };

            foreach (var platformType in platformTypes)
            {
                context.PlatformTypes.Add(platformType);
            }

            var games = new List<Game>();
            var rnd = new Random();

            for (int i = 1; i <= 150; i++)
            {
                var genreId = rnd.Next(genres.Count());
                var game = new Game
                {
                    Key = "Game" + i,
                    Name = "Game " + i,
                    Translations = new List<GameTranslation>
                    {
                        new GameTranslation { Description = "Description game " + i, Language = Language.En}, 
                        new GameTranslation { Description = "Описание игры " + i, Language = Language.Ru},
                    },
                    ContentType = "application/zip",
                    Price = (decimal) rnd.NextDouble()*1000,
                    Genres = new List<Genre>{genres[genreId], genres[(genreId+1)%genres.Count()]},
                    UnitsInStock = (short) rnd.Next(30),
                    Discontinued = true,
                    Publisher = publishers[rnd.Next(publishers.Count())],
                    PlatformTypes = new List<PlatformType>{platformTypes[rnd.Next(platformTypes.Length)], platformTypes[rnd.Next(platformTypes.Length)]},
                    CreationDate = DateTime.UtcNow.AddDays(-rnd.Next(100)),
                    PublishedDate = DateTime.UtcNow.AddDays(-rnd.Next(1000))
                };
                games.Add(game);
            }

            foreach (var game in games)
            {
                context.Games.Add(game);
            }

            var comments = new[]
            {
                new Comment
                {
                    Game = games[0],
                    Body = "My comment1",
                    Name = users[3].Name,
                    User = users[3]
                },
                new Comment
                {
                    Game = games[0],
                    Body = "My comment2",
                    Name = users[4].Name,
                    User = users[4]
                },
                new Comment
                {
                    Game = games[1],
                    Body = "My comment3",
                    Name = users[3].Name,
                    User = users[3]
                },
                new Comment
                {
                    Game = games[1],
                    Body = "My comment4",
                    Name = users[4].Name,
                    User = users[4]
                }
            };

            var commentChilds = new[]
            {
                new Comment
                {
                    Game = games[0],
                    Body = "My comment5",
                    Name = users[4].Name,
                    User = users[4],
                    Parent = comments[1]
                },
                new Comment
                {
                    Game = games[0],
                    Body = "My comment6",
                    Name = users[4].Name,
                    User = users[4],
                    Parent = comments[0]
                },
                new Comment
                {
                    Game = games[1],
                    Body = "My comment7",
                    Name = users[3].Name,
                    User = users[3],
                    Parent = comments[2]
                }
            };

            foreach (var comment in comments)
            {
                context.Comments.Add(comment);
            }

            foreach (var child in commentChilds)
            {
                context.Comments.Add(child);
            }

            var uniques = new[]
            {
                new {Table = "Games", Field = "Key"},
                new {Table = "GenreTranslations", Field = "Name"},
                new {Table = "PlatformTypes", Field = "Type"},
                new {Table = "Publishers", Field = "CompanyName"},
                new {Table = "Roles", Field = "Name"}
            };

            foreach (var i in uniques)
            {
                context.Database.ExecuteSqlCommand(string.Format("CREATE UNIQUE INDEX {0}_{1} ON [{0}] ([{1}]);", i.Table, i.Field));
            }

            var payments = new[]
            {
                new Payment {Type = PaymentType.Bank, Name = "Pay by bank", Description = "Pay your order in bank"},
                new Payment {Type = PaymentType.Ibox, Name = "Pay by IBox", Description = "Pay your order in IBox"},
                new Payment {Type = PaymentType.Visa, Name = "Pay by visa", Description = "Pay your order by visa card"},
                new Payment {Type = PaymentType.MasterCard, Name = "Pay by master card", Description = "Pay your order by master card"}
            };
            foreach (var payment in payments)
            {
                context.Payments.Add(payment);
            }

            base.Seed(context);
        }
    }
}