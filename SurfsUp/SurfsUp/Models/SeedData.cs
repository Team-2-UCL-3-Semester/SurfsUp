using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SurfsUp.Data;
using SurfsUp.Models;
using System;
using System.Linq;

namespace SeedDa
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new SurfsUpContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<SurfsUpContext>>()))
            {
                // Look for any Boards.
                if (context.Board.Any())
                {
                    return;   // DB has been seeded
                }

                context.Board.AddRange(
                    new Board
                    {
                        Name = "The Minilog",
                        Length = 6.0M,
                        Width = 21.0M,
                        Thickness = 2.75M,
                        Volume = 38.8M,
                        Type = "Shortboard",
                        Price = 565.0M,
                        Equipment = "",
                        imgPath = ""
                    },

                    new Board
                    {
                        Name = "The Wide Glider",
                        Length = 7.1M,
                        Width = 21.75M,
                        Thickness = 2.75M,
                        Volume = 44.16M,
                        Type = "Funboard",
                        Price = 685.0M,
                        Equipment = "",
                        imgPath = ""
                    },

                    new Board
                    {
                        Name = "The Golden Ratio",
                        Length = 6.3M,
                        Width = 21.85M,
                        Thickness = 2.9M,
                        Volume = 43.22M,
                        Type = "Funboard",
                        Price = 695.0M,
                        Equipment = "",
                        imgPath = ""
                    },

                    new Board
                    {
                        Name = "Mahi Mahi",
                        Length = 5.4M,
                        Width = 20.75M,
                        Thickness = 2.3M,
                        Volume = 29.39M,
                        Type = "Fish",
                        Price = 645.0M,
                        Equipment = "",
                        imgPath = ""
                    },

                    new Board
                    {
                        Name = "The Emerald Glider",
                        Length = 9.2M,
                        Width = 22.8M,
                        Thickness = 2.8M,
                        Volume = 65.4M,
                        Type = "Longboard",
                        Price = 895.0M,
                        Equipment = "",
                        imgPath = ""
                    },
                    new Board
                    {
                        Name = "The Bomb",
                        Length = 5.5M,
                        Width = 21.0M,
                        Thickness = 2.5M,
                        Volume = 38.8M,
                        Type = "Shortboard",
                        Price = 565.0M,
                        Equipment = "",
                        imgPath = ""
                    },
                    new Board
                    {
                        Name = "The Minilog",
                        Length = 6.0M,
                        Width = 21.0M,
                        Thickness = 2.75M,
                        Volume = 38.8M,
                        Type = "Shortboard",
                        Price = 565.0M,
                        Equipment = "",
                        imgPath = ""
                    },
                    new Board
                    {
                        Name = "The Minilog",
                        Length = 6.0M,
                        Width = 21.0M,
                        Thickness = 2.75M,
                        Volume = 38.8M,
                        Type = "Shortboard",
                        Price = 565.0M,
                        Equipment = "",
                        imgPath = ""
                    },
                    new Board
                    {
                        Name = "The Minilog",
                        Length = 6.0M,
                        Width = 21.0M,
                        Thickness = 2.75M,
                        Volume = 38.8M,
                        Type = "Shortboard",
                        Price = 565.0M,
                        Equipment = "",
                        imgPath = ""
                    },
                    new Board
                    {
                        Name = "The Minilog",
                        Length = 6.0M,
                        Width = 21.0M,
                        Thickness = 2.75M,
                        Volume = 38.8M,
                        Type = "Shortboard",
                        Price = 565.0M,
                        Equipment = "",
                        imgPath = ""
                    }


                );
                context.SaveChanges();
            }
        }
    }
}