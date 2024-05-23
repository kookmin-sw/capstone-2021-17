﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Popcron.Console
{
    public sealed class Library
    {
        private static List<(Assembly assembly, Type[] types)> assemblies = null;
        private static List<Command> commands = null;
        private static List<Category> categories = null;

        public static List<Command> Commands
        {
            get
            {
                if (commands == null)
                {
                    FindCommands();
                }

                return commands;
            }
        }

        public static List<Category> Categories
        {
            get
            {
                if (categories == null)
                {
                    FindCategories();
                }

                return categories;
            }
        }

        private static List<(Assembly assembly, Type[] types)> Assemblies
        {
            get
            {
                if (assemblies == null)
                {
                    List<string> allAssemblies = Settings.Current.assemblies.ToList();
                    Assembly executingAssembly = Assembly.GetExecutingAssembly();
                    Assembly callingAssembly = Assembly.GetCallingAssembly();
                    Assembly entryAssembly = Assembly.GetEntryAssembly();
                    Assembly consoleAssembly = typeof(ConsoleWindow).Assembly;

                    //ensure the last 4 assemblies exist in the list
                    if (executingAssembly != null && !allAssemblies.Contains(executingAssembly.FullName))
                    {
                        allAssemblies.Add(executingAssembly.FullName);
                    }

                    if (callingAssembly != null && !allAssemblies.Contains(callingAssembly.FullName))
                    {
                        allAssemblies.Add(callingAssembly.FullName);
                    }

                    if (entryAssembly != null && !allAssemblies.Contains(entryAssembly.FullName))
                    {
                        allAssemblies.Add(entryAssembly.FullName);
                    }

                    if (consoleAssembly != null && !allAssemblies.Contains(consoleAssembly.FullName))
                    {
                        allAssemblies.Add(consoleAssembly.FullName);
                    }

                    assemblies = new List<(Assembly assembly, Type[] types)>();
                    for (int a = 0; a < allAssemblies.Count; a++)
                    {
                        try
                        {
                            Assembly assembly = Assembly.Load(allAssemblies[a]);
                            if (assembly != null)
                            {
                                Type[] types = assembly.GetTypes();
                                assemblies.Add((assembly, types));
                            }
                        }
                        catch
                        {

                        }
                    }
                }

                return assemblies;
            }
        }

        public static void FindCategories()
        {
            if (categories == null)
            {
                categories = new List<Category>();
                HashSet<Type> typesWithoutCategories = new HashSet<Type>();
                for (int a = 0; a < Assemblies.Count; a++)
                {
                    Type[] types = Assemblies[a].types;
                    for (int t = 0; t < types.Length; t++)
                    {
                        Type type = types[t];
                        Category category = Category.Create(type);
                        if (category != null)
                        {
                            if (TryGetCategory(category.Name, out Category existingCategory))
                            {
                                existingCategory.Commands.AddRange(category.Commands);
                            }
                            else
                            {
                                categories.Add(category);
                            }
                        }
                        else
                        {
                            typesWithoutCategories.Add(type);
                        }
                    }
                }

                //sort alphabetically
                categories = categories.OrderBy(x => x.Name).ToList();

                if (typesWithoutCategories.Count > 0)
                {
                    Category uncategorized = Category.CreateUncategorized();
                    List<Command> commands = Commands;
                    foreach (Command command in commands)
                    {
                        if (typesWithoutCategories.Contains(command.Owner))
                        {
                            uncategorized.Commands.Add(command);
                        }
                    }

                    if (uncategorized.Commands.Count > 0)
                    {
                        categories.Add(uncategorized);
                    }
                }
            }
        }

        public static bool TryGetCategory(string name, out Category category)
        {
            category = null;
            if (categories == null)
            {
                return false;
            }

            for (int i = 0; i < categories.Count; i++)
            {
                if (categories[i].Name == name)
                {
                    category = categories[i];
                    return true;
                }
            }

            return false;
        }

        public static void AddCommand(Command command, string category = null)
        {
            if (commands == null)
            {
                FindCommands();
            }

            if (!TryGetCategory(category, out Category cat))
            {
                cat = Categories[Categories.Count - 1];
            }

            foreach (Command existingCommand in cat.Commands)
            {
                if (existingCommand.GetHashCode() == command.GetHashCode())
                {
                    //already exists!
                    return;
                }
            }

            cat.Commands.Add(command);
            commands.Add(command);
        }

        public static void FindCommands()
        {
            if (commands == null)
            {
                commands = new List<Command>();
                for (int a = 0; a < Assemblies.Count; a++)
                {
                    Type[] types = Assemblies[a].types;
                    for (int t = 0; t < types.Length; t++)
                    {
                        Type type = types[t];
                        MethodInfo[] methods = type.GetMethods();
                        for (int m = 0; m < methods.Length; m++)
                        {
                            MethodInfo method = methods[m];
                            Command command = Command.Create(method, type);
                            if (command != null)
                            {
                                commands.Add(command);
                            }
                        }

                        PropertyInfo[] properties = type.GetProperties();
                        for (int p = 0; p < properties.Length; p++)
                        {
                            PropertyInfo property = properties[p];
                            Command command = Command.Create(property, type);
                            if (command != null)
                            {
                                commands.Add(command);
                            }
                        }

                        FieldInfo[] fields = type.GetFields();
                        for (int f = 0; f < fields.Length; f++)
                        {
                            FieldInfo field = fields[f];
                            Command command = Command.Create(field, type);
                            if (command != null)
                            {
                                commands.Add(command);
                            }
                        }
                    }
                }
            }
        }
    }
}
