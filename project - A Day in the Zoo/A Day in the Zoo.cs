using System;
using System.Collections.Generic;
using System.Timers;

namespace AG_ZOO
{

    class User
    {
        public static string read_string(string prompt)
        {
            Console.Write(prompt);
            return Console.ReadLine();
        }
        public static int read_integer(string prompt) // checks the input from user 
        {
            while (true)
            {
                try
                {
                    Console.Write(prompt);
                    return Convert.ToInt32(Console.ReadLine());
                }
                catch
                {
                    colored_text("please enter only integer values.", ConsoleColor.Red);
                }
            }
        }
        public static int read_integer_range(string prompt, int min, int max)
        {
            int input = read_integer(prompt);

            while (input < min || input > max)
            {
                colored_text("please enter an integer within range " + (min) + " and " + (max), ConsoleColor.Red);

                input = read_integer(prompt);
            }
            return input;
        }
        public static int read_integer_pos(string prompt) // checks the input from user 
        {
            while (true)
            {
                int input = read_integer(prompt);
                if (input > 0)
                    return input;
                else
                    colored_text("please enter only integer values.", ConsoleColor.Red);
            }
        }
        public static double read_double(string prompt)
        {
            while (true)
            {
                try
                {
                    Console.Write(prompt);
                    return Convert.ToDouble(Console.ReadLine());
                }
                catch
                {
                    colored_text("please enter only double values.", ConsoleColor.Red);
                }
            }
        }
        public static double read_double_pos(string prompt) // checks the input from user 
        {
            while (true)
            {
                double input = read_double(prompt);
                if (input > 0)
                    return input;
                else
                    colored_text("\nplease enter positive values only...", ConsoleColor.Red);
            }
        }
        public static bool read_boolean(string prompt)
        {
            string input; bool ans;
            Console.Write(prompt);

            while (true)
            {
                input = Console.ReadLine();
                input = (input.Trim()).ToLowerInvariant();

                if (input == "yes" || input == "y" || input == "1")
                {
                    ans = true;
                    return ans;
                }
                else if (input == "no" || input == "n" || input == "0")
                {
                    ans = false;
                    return ans;
                }
                else
                {
                    colored_text(" please enter either \'yes\' or \'no\' or \'1/0\' >> ", ConsoleColor.Red);
                }
            }
        }
        public static void colored_text(string prompt, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(prompt);
            Console.ResetColor();
        }
    }
    class Admin
    {
        private string name, password;
        private List<Visitor> visitor_log;
        private List<Staff> staffList;
        private Animal_log animals;
        private Inventory inventory;
        private double revenue;
        private static System.Timers.Timer Timer;

        public string Name { get => name; set => name = value; }
        public string Password { get => password; set => password = value; }
        internal List<Visitor> Visitor_log { get => visitor_log; }
        internal List<Staff> StaffList { get => staffList; }
        internal Animal_log Animals { get => animals; }
        internal Inventory Inventory { get => inventory; }
        public double Revenue { get => revenue; set => revenue = value; }

        public Admin(string name, string password, double revenue)
        {
            visitor_log = new List<Visitor>();
            staffList = new List<Staff>();
            animals = new Animal_log();
            inventory = new Inventory();

            this.Name = name;
            this.Password = password;
            this.Revenue = revenue;

            for (var i = 0; i < Staff.Staff_size; i++)
            {
                StaffList.Add(new Staff());
            }

            SetTimer();
        }

        public void take_revenue(double sum)
        {
            if (Revenue <= sum || Animals.List.Count == 0)
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Its really sad to say but\nthe Zoo has gone bankrupt and will be shut down\nsorry for inconvenience.");
                Console.ResetColor();
                Environment.Exit(-1);
            }

            Revenue -= sum;
        }

        public void give_revenue(double sum)
        {
            Revenue += sum;
        }

        public void get_staffdetails()
        {
            Console.WriteLine("    | {0,-20} | {1,-5} | {2,10} | {3,-10} ", "name", "age", "gender", "animal");

            for (var i = 0; i < StaffList.Count; i++)
            {
                StaffList[i].details(i);
            }
        }
        private void SetTimer()
        {
            Timer = new System.Timers.Timer(30000);
            // Hook up the Elapsed event for the Timer. 
            Timer.Elapsed += OnTimedEvent;
            Timer.AutoReset = true;
            Timer.Enabled = true;
        }

        private void OnTimedEvent(Object source, EventArgs e)
        {
            Animals.events();
            Animals.count_down(this);

        }
    }
    class Staff
    {
        private const int staff_size = 7;
        private string name, gender;
        private int age;
        private Animal animal;
        private string[] M_name = { "Liam", "Noah", "Oliver", "William", "Elijah", "James", "Ben", "Lucas", "Mason", "Ethan" };
        private string[] F_name = { "Olivia", "Emma", "Ava", "Sophia", "Isabel", "scarlet", "Amelia", "Mia", "Harper", "Evelyn" };
        private string[] L_name = { "Smith", "Johnson", "Williams", "Brown", "Jones", "Garcia", "Miller", "Davis", "Heffley", "Martinez" };

        public static int Staff_size => staff_size;

        public Staff()
        {
            Random rnd = new Random();

            if (rnd.Next(10) < 5)
            {
                gender = "Male";
                name = M_name[rnd.Next(0, M_name.Length - 1)];
                name += " " + L_name[rnd.Next(0, L_name.Length - 1)];
            }
            else
            {
                gender = "Female";
                name = F_name[rnd.Next(0, F_name.Length - 1)];
                name += " " + L_name[rnd.Next(0, L_name.Length - 1)];
            }
            age = rnd.Next(18, 40);
            animal = null;
        }
        public void details(int i)
        {
            string animal_data = animal == null ? "not assigned" : animal.Name + " the " + animal.Species;
            Console.WriteLine("{4,2}. | {0,-20} | {1,-5} | {2,10} | {3,-20} ", name, age, gender, animal_data, (i + 1));
        }

        public void assigned(Animal animal)
        {
            this.animal = animal;
            this.animal.Assigned = true;
        }
    }
    public enum food_type
    {
        VEG, NON_VEG
    }
    class Medicine
    {
        private string name;
        private double cost;
        private int amount;

        public string Name { get => name; }
        public double Cost { get => cost; }
        public int Amount { get => amount; set => amount = value; }

        public Medicine(string name, double cost)
        {
            this.name = name;
            this.cost = cost;
            Amount = 1;
        }
    }
    class Food
    {
        private string name;
        private double cost;
        private food_type type;
        private int amount;

        public string Name { get => name; }
        public double Cost { get => cost; }
        public food_type Type { get => type; }
        public int Amount { get => amount; set => amount = value; }

        public Food(string name, double cost, food_type type)
        {
            this.name = name;
            this.cost = cost;
            this.type = type;
            Amount = 1;
        }
    }

    class Supply_store
    {
        private List<Medicine> medicines;
        private List<Food> foodlist;

        internal List<Medicine> Medicines { get => medicines; }
        internal List<Food> Foodlist { get => foodlist; }

        public Supply_store()
        {

            medicines = new List<Medicine>();

            foodlist = new List<Food>();

            Medicines.Add(new Medicine("Gentamicin", 22.99));
            Medicines.Add(new Medicine("Clindamycin", 28.82));
            Medicines.Add(new Medicine("Amoxicillin", 25.32));

            Foodlist.Add(new Food("watermelon", 4.90, food_type.VEG));
            Foodlist.Add(new Food("banana", 1.48, food_type.VEG));
            Foodlist.Add(new Food("cabbage", 2.76, food_type.VEG));
            Foodlist.Add(new Food("apple", 1.32, food_type.VEG));
            Foodlist.Add(new Food("eggs", 2.75, food_type.NON_VEG));
            Foodlist.Add(new Food("meat", 6.45, food_type.NON_VEG));
        }
        public void print_med_list()
        {
            Console.WriteLine("\n    {0,-12} | {1,5}", "NAME", "COST");

            for (var i = 0; i < Medicines.Count; i++)
            {
                Console.WriteLine("{2,2}. {0,-12} | {1,5}", Medicines[i].Name, Medicines[i].Cost, (i + 1));
            }
            Console.WriteLine();
        }

        public void print_food_list()
        {
            Console.WriteLine("\n    {0,-10} | {1,-5} | {2,10}", "NAME", "COST", "FOOD TYPE");

            for (var i = 0; i < Foodlist.Count; i++)
            {
                Console.WriteLine("{3,2}. {0,-10} | {1,-5} | {2,10}", Foodlist[i].Name, Foodlist[i].Cost, Foodlist[i].Type, (i + 1));
            }
            Console.WriteLine();
        }

    }
    class Inventory
    {
        private List<Medicine> medicines;
        private List<Food> foodlist;
        private Supply_store order_list;

        internal List<Medicine> Medicines { get => medicines; }
        internal List<Food> Foodlist { get => foodlist; }
        internal Supply_store Order_list { get => order_list; }

        public Inventory()
        {
            medicines = new List<Medicine>();
            foodlist = new List<Food>();
            order_list = new Supply_store();
        }
        public void get_inventory_med()
        {
            if (Medicines.Count == 0)
                throw new InvalidOperationException("\nmedicine supply is exhausted right now.\nBuy some to view\n");

            for (var i = 0; i < Medicines.Count; i++)
            {
                Console.WriteLine((i + 1) + ". {0,-12} x {1,5}", Medicines[i].Name, Medicines[i].Amount);
            }
        }
        public void get_inventory_food()
        {
            if (Foodlist.Count == 0)
                throw new InvalidOperationException("\nfood supply is exhausted right now.\nBuy some to view\n");

            for (var i = 0; i < Foodlist.Count; i++)
            {
                Console.WriteLine((i + 1) + ". {0,-10} x {1,5}", Foodlist[i].Name, Foodlist[i].Amount);
            }
        }
        public void add_inventory(string choice, Admin admin)
        {
            int num, quant, index;

            if (choice == "medicine")
            {
                Order_list.print_med_list();
                num = User.read_integer_range("enter the medicine you want to buy >> ", 1, Order_list.Medicines.Count);
                quant = User.read_integer_pos("enter the quantity required >> ");

                admin.take_revenue(quant * Order_list.Medicines[num - 1].Cost);

                if (Medicines.Contains(Order_list.Medicines[num - 1]))
                {
                    index = Medicines.IndexOf(Order_list.Medicines[num - 1]);
                    Medicines[index].Amount += quant;
                }
                else
                {
                    Medicine temp = Order_list.Medicines[num - 1];
                    temp.Amount = quant;
                    Medicines.Add(temp);
                }
            }

            else if (choice == "food")
            {
                Order_list.print_food_list();
                num = User.read_integer_range("enter the food item you want to buy >> ", 1, Order_list.Foodlist.Count);
                quant = User.read_integer_pos("enter the quantity required >> ");

                admin.take_revenue(quant * Order_list.Foodlist[num - 1].Cost);

                if (Foodlist.Contains(Order_list.Foodlist[num - 1]))
                {
                    index = Foodlist.IndexOf(Order_list.Foodlist[num - 1]);
                    Foodlist[index].Amount += quant;
                }
                else
                {
                    Food temp = Order_list.Foodlist[num - 1];
                    temp.Amount = quant;
                    Foodlist.Add(temp);
                }
            }
        }
        public Medicine use_med()
        {
            if (Medicines.Count == 0)
                throw new InvalidOperationException("\nmedicine supply is exhausted right now.\nBuy some to use\n");
            int num;

            get_inventory_med();
            num = User.read_integer_range("enter the medicine you want to use >> ", 1, Order_list.Medicines.Count);

            return Medicines[num - 1];
        }
        public Food use_food()
        {
            if (Foodlist.Count == 0)
                throw new InvalidOperationException("\nfood supply is exhausted right now.\nBuy some to use\n");
            int num;

            get_inventory_food();
            num = User.read_integer_range("enter the food item you want to use >> ", 1, Order_list.Foodlist.Count);

            return Foodlist[num - 1];
        }
        public void remove_inventory(Medicine med)
        {
            int num;

            num = Medicines.IndexOf(med);

            if (Medicines[num].Amount == 1)
            {
                Medicines.RemoveAt(num);
            }
            else
                Medicines[num].Amount--;
        }
        public void remove_inventory(Food food)
        {
            int num;

            num = Foodlist.IndexOf(food);

            if (Foodlist[num].Amount == 1)
            {
                Foodlist.RemoveAt(num);
            }
            else
                Foodlist[num].Amount--;
        }
    }

    abstract class Animal
    {
        private string location, name, species;
        private double weight;
        private int age, cage_num, countdown;
        private bool is_healthy, is_hungry, assigned;
        protected food_type food_Type;

        public string Name { get => name; }
        public string Species { get => species; }
        public string Location { get => location; }
        public int Age { get => age; }
        public int Cage_num { get => cage_num; }
        public food_type Food_Type { get => food_Type; }
        public double Weight { get => weight; }
        public bool Is_healthy { get => is_healthy; set => is_healthy = value; }
        public bool Is_hungry { get => is_hungry; set => is_hungry = value; }
        public int Countdown { get => countdown; set => countdown = value; }
        public bool Assigned { get => assigned; set => assigned = value; }

        public Animal(string name, string location, string species, int encl_num, int approx)
        {
            this.name = name;
            this.location = location;
            this.cage_num = encl_num;
            this.species = species;
            Assigned = false;
            Countdown = 0;
            Random rnd = new Random();

            weight = rnd.Next(approx, approx * 2);
            age = rnd.Next(approx / 10, (approx * 2) / 10);

            Is_healthy = true;
            Is_hungry = false;
        }
        public void details_admin()
        {
            Console.WriteLine("{0} is currently {1} ", Name, Is_healthy == true ? "healthy and happy" : "sick and needs meds");
            Console.WriteLine("{0} is currently {1} ", Name, Is_hungry == true ? "hungry and needs food" : "satisfied and joyous");
        }
        public void details_visitor()
        {
            Console.Write($"{Name} is a {Species}, it is {Age} year(s) old and weighs around {Weight} kg(s) and is {Food_Type} by nature");
            Console.ReadLine();
            Console.Write($"normally {Species}s can be found in {Location}. {Name} lives in enclosure no. {Cage_num}\n");
            Console.ReadLine();

        }

        public void use(Medicine meds)
        {
            if (!Assigned)
                throw new InvalidOperationException($"please first, assign a worker to take care of {Name}'s health");

            if (Is_healthy)
                throw new InvalidOperationException("\n animal is in perfect health right now");

            Is_healthy = true;
        }
        public void use(Food food)
        {
            if (!Assigned)
                throw new InvalidOperationException($"please first, assign a worker to take care of {Name}'s hunger");

            if (food.Type != Food_Type)
                throw new InvalidOperationException($"\n animal does not eat {food.Type} food");

            if (!Is_hungry)
                throw new InvalidOperationException("\n animal not hungry right now");

            Is_hungry = false;

        }
    }

    class Carnivore : Animal
    {
        public Carnivore(string name, string location, string species, int encl_num, int approx) : base(name, location, species, encl_num, approx)
        {
            this.food_Type = food_type.NON_VEG;
        }
    }

    class Herbivore : Animal
    {
        public Herbivore(string name, string location, string species, int encl_num, int approx) : base(name, location, species, encl_num, approx)
        {
            this.food_Type = food_type.VEG;

        }
    }

    class Animal_log
    {
        private List<Animal> list;

        internal List<Animal> List { get => list; }

        public Animal_log()
        {
            list = new List<Animal>();
            
            List.Add(new Carnivore("Sher Khan", "Bengal", "Tiger", 1, 100));
            List.Add(new Carnivore("Babbar", "Africa", "Lion", 2, 110));
            List.Add(new Herbivore("Roger", "Australia", "Kangaroo", 3, 50));
            List.Add(new Herbivore("Skipper", "Antarctic", "Penguin", 4, 10));

        }

        public void get_details()
        {
            Console.WriteLine("\n    | {0,-10} | {1,-10} | {2,5} | {3,7} | {4,10} | {5,10} | {6,3} |", "NAME", "SPECIES", "AGE", "WEIGHT", "FROM", "Food type", " CAGE");

            for (var i = 0; i < List.Count; i++)
            {
                Console.WriteLine("{7,2}. | {0,-10} | {1,-10} | {2,5} | {3,7} | {4,10} | {5,10} | {6,5} |",
                                            List[i].Name, List[i].Species, List[i].Age, List[i].Weight, List[i].Location, List[i].Food_Type, List[i].Cage_num, (i + 1));

            }
            Console.WriteLine();
        }

        public void events()
        {
            Random rnd = new Random();

            foreach (var animal in List)
            {
                if (animal.Is_healthy && rnd.Next(10) > 7)

                    animal.Is_healthy = false;

                if (!animal.Is_hungry && rnd.Next(10) > 7)

                    animal.Is_hungry = true;

                if (animal.Is_hungry || !animal.Is_healthy)
                    animal.Countdown++;

                else animal.Countdown = 0;

            }
        }

        public void count_down(Admin admin)
        {
            foreach (var animal in List)
            {
                if (animal.Countdown == 7)
                {
                    User.colored_text("\nplease check up on the animals\n", ConsoleColor.Green);
                }
                else if (animal.Countdown >= 10)
                {
                    User.colored_text($"\nsorry but you could not take care of {animal.Name},", ConsoleColor.Green);
                    User.colored_text("so it is transferred to another zoo. please take good care", ConsoleColor.Green);
                    User.colored_text("of the remaining animals. (Ps - fine of $2000 is imposed on the zoo ", ConsoleColor.Green);
                    List.Remove(animal);
                    admin.take_revenue(2000);
                }
            }
        }

        public List<String> print_species()
        {
            List<String> species = new List<string>();
            int num = 1;

            for (var i = 0; i < List.Count; i++)
            {
                if (!species.Contains(List[i].Species))
                {
                    Console.WriteLine(num++ + ". " + List[i].Species);
                    species.Add(List[i].Species);
                }
            }
            return species;
        }

        public int animal_choose()
        {
            int choice = 0;
            List<String> species;
            Console.WriteLine();
            Console.WriteLine("which animal you would like to see first.");
            species = print_species();
            Console.WriteLine();

            choice = User.read_integer_range(">>>> ", 1, 4);

            for (var i = 0; i < List.Count; i++)
            {
                if (List[i].Species == species[choice - 1])
                    return i;
            }
            return 0;
        }
    }

    class Visitor
    {
        private string name;
        private double wallet;
        private int photos;
        private bool has_camera;

        public double Wallet { get => wallet; }
        public int Photos { get => photos; set => photos = value; }

        public Visitor(Admin admin)
        {
            name = User.read_string("enter your name >> ");
            wallet = User.read_double_pos("enter the money you have >> ");

            has_camera = User.read_boolean("Do you have a camera? (Y/N) >> ");

            if (!has_camera && User.read_boolean("Want to rent one?\n $50 dollars only\t(Y/N) >> "))
            {
                check_wallet(50, admin);
                has_camera = !has_camera;
            }
        }

        public void check_wallet(double amount, Admin admin)
        {
            if (Wallet < amount)
                throw new InvalidOperationException("insufficient amount in wallet");

            wallet -= amount;
            admin.give_revenue(amount);
        }
    }
    class Menu
    {

        public static void _visitor_tour(Animal_log log, Visitor visitor, Admin admin)
        {

            int num = log.animal_choose();
            int choice = 0;
            do
            {
                try
                {
                    Console.WriteLine();
                    Console.WriteLine($"enter 1: get details of {log.List[num].Name} the {log.List[num].Species}");
                    Console.WriteLine("enter 2: click picture");
                    Console.WriteLine("enter 3: Donation for animal welfare");
                    Console.WriteLine("enter 4: Look for other animals");
                    Console.WriteLine("enter 5: To take a break");
                    Console.WriteLine();

                    choice = User.read_integer_range("enter your choice >> ", 1, 5);

                    switch (choice)
                    {
                        case 1:
                            log.List[num].details_visitor();
                            break;
                        case 2:
                            Display.animal(log.List[num].Species);
                            visitor.Photos++;
                            break;
                        case 3:
                            double amount = User.read_double_pos($"enter amount to donate to {log.List[num].Name} >> ");
                            visitor.check_wallet(amount, admin);
                            break;
                        case 4:
                            num = log.animal_choose();
                            break;
                        case 5:
                            Console.WriteLine("finding a place to rest...");
                            break;
                    }
                }
                catch (System.Exception ex)
                {
                    User.colored_text(ex.Message, ConsoleColor.Red);
                }
            } while (choice != 5);
        }
        public static void _Visitor(Admin admin) // used to give menu to the user
        {
            admin.Visitor_log.Add(new Visitor(admin));

            int current = admin.Visitor_log.Count - 1;
            int choice = 0;
            do
            {
                try
                {

                    Console.WriteLine();
                    Console.WriteLine("enter 1: explore Zoo");
                    Console.WriteLine("enter 2: have a refreshment");
                    Console.WriteLine("enter 3: check money available");
                    Console.WriteLine("enter 4: check total photos clicked");
                    Console.WriteLine("enter 5: Call it a day.");
                    Console.WriteLine();

                    choice = User.read_integer_range("enter your choice >> ", 1, 5);

                    switch (choice)
                    {
                        case 1:
                            _visitor_tour(admin.Animals, admin.Visitor_log[current], admin);
                            break;

                        case 2:
                            if (User.read_boolean("Do you want to buy refreshment?\nfor $10\t (Y/N) >> "))
                            {
                                admin.Visitor_log[current].check_wallet(10, admin);
                                Console.WriteLine("Refreshment done.");
                            }
                            break;

                        case 3:
                            Console.WriteLine($"Money left >> {admin.Visitor_log[current].Wallet}");
                            break;

                        case 4:
                            Console.WriteLine($"Total pictures taken >> {admin.Visitor_log[current].Photos}");
                            break;

                        case 5:
                            User.colored_text("Thanks for coming\nsee you again soon", ConsoleColor.Yellow);
                            break;
                    }
                }
                catch (System.Exception ex)
                {
                    User.colored_text(ex.Message, ConsoleColor.Red);
                }
            } while (choice != 5);
        }
        public static void _Animal(Admin admin) // used to give menu to the user
        {
            int choice = 0;
            int current = User.read_integer_range("enter the animal you want to select >> ", 1, admin.Animals.List.Count) - 1;
            do
            {
                try
                {

                    Console.WriteLine();
                    Console.WriteLine("enter 1: view details");
                    Console.WriteLine("enter 2: give medicine");
                    Console.WriteLine("enter 3: give food");
                    Console.WriteLine("enter 4: assign staff");
                    Console.WriteLine("enter 5: to quit");
                    Console.WriteLine();

                    choice = User.read_integer_range("enter your choice >> ", 1, 5);

                    switch (choice)
                    {
                        case 1:
                            admin.Animals.List[current].details_admin();
                            break;

                        case 2:
                            Medicine med = admin.Inventory.use_med();

                            admin.Animals.List[current].use(med);
                            admin.Inventory.remove_inventory(med);

                            Console.WriteLine("Medication done.");

                            break;

                        case 3:
                            Food food = admin.Inventory.use_food();

                            admin.Animals.List[current].use(food);
                            admin.Inventory.remove_inventory(food);

                            Console.WriteLine("Animal feeding done.");

                            break;

                        case 4:
                            admin.get_staffdetails();
                            int num = User.read_integer_range("enter the staff member you want to assign >> ", 1, admin.StaffList.Count) - 1;
                            admin.StaffList[num].assigned(admin.Animals.List[current]);
                            break;

                        case 5:
                            Console.WriteLine("back to admin menu...");
                            break;
                    }
                }
                catch (System.Exception ex)
                {
                    User.colored_text(ex.Message, ConsoleColor.Red);

                }
            } while (choice != 5);
        }
        public static void _inventory(Inventory inventory, Admin admin) // used to give menu to the user
        {
            int choice = 0;
            do
            {
                try
                {
                    Console.WriteLine("\nenter 1: medicines supply details");
                    Console.WriteLine("enter 2: buy medicines");

                    Console.WriteLine("\nenter 3: food supply details");
                    Console.WriteLine("enter 4: buy food");
                    Console.WriteLine("enter 5: to quit\n");

                    choice = User.read_integer_range("enter your choice >> ", 1, 5);

                    switch (choice)
                    {
                        case 1:
                            inventory.get_inventory_med();
                            break;

                        case 2:
                            inventory.add_inventory("medicine", admin);
                            break;

                        case 3:

                            inventory.get_inventory_food();
                            break;

                        case 4:
                            inventory.add_inventory("food", admin);
                            break;

                        case 5:
                            Console.WriteLine("back to admin menu...");
                            break;
                    }
                }
                catch (System.Exception ex)
                {
                    User.colored_text(ex.Message, ConsoleColor.Red);

                }
            } while (choice != 5);
        }
        public static void _Admin(Admin admin) // used to give menu to the user
        {
            int choice = 0;
            do
            {
                try
                {
                    Console.WriteLine();
                    Console.WriteLine("enter 1: manage inventory");
                    Console.WriteLine("enter 2: view staff details");
                    Console.WriteLine("enter 3: select animal");
                    Console.WriteLine("enter 4: view revenue");
                    Console.WriteLine("enter 5: to quit");
                    Console.WriteLine();

                    choice = User.read_integer_range("enter your choice >> ", 1, 5);

                    switch (choice)
                    {
                        case 1:
                            _inventory(admin.Inventory, admin);
                            break;

                        case 2:
                            admin.get_staffdetails();
                            break;

                        case 3:
                            admin.Animals.get_details();
                            _Animal(admin);
                            break;

                        case 4:
                            Console.WriteLine($"Money left >> {admin.Revenue}");
                            break;

                        case 5:
                            Console.WriteLine("back to role select menu...");
                            break;
                    }
                }
                catch (System.Exception ex)
                {
                    User.colored_text(ex.Message, ConsoleColor.Red);
                }
            } while (choice != 5);
        }
        public static void _Main(Admin admin) // used to give menu to the user
        {
            int choice = 0;
            do
            {
                try
                {
                    Console.WriteLine();
                    Console.WriteLine("enter 1: for playing as Administrator");
                    Console.WriteLine("enter 2: for playing as visitor");
                    Console.WriteLine("enter 3: quit");
                    Console.WriteLine();

                    choice = User.read_integer_range("enter your choice >> ", 1, 3);

                    switch (choice)
                    {
                        case 1:

                            if (admin.Name == string.Empty)
                            {
                                admin.Name = User.read_string("\nenter the name of admin >> ");
                                admin.Password = User.read_string("enter a strong password >> ");
                                admin.Revenue = User.read_integer_pos("enter starting revenue >> ");
                            }
                            else
                                while (admin.Password != User.read_string($"enter administrator password (For {admin.Name})>> "))
                                {
                                    User.colored_text("\nincorrect password, try again\n", ConsoleColor.Red);
                                }

                            User.colored_text("\nWelcome to Administrative section of ZOO\n", ConsoleColor.Cyan);
                            _Admin(admin);

                            break;
                        case 2:
                            _Visitor(admin);

                            break;
                    }
                }
                catch (System.Exception ex)
                {
                    User.colored_text(ex.Message, ConsoleColor.Red);
                }
            } while (choice != 3);
        }
    }
    class Game_play
    {
        public static void Main(string[] args)
        {
            Display.welcome();

            User.read_string("PRESS ENTER TO PLAY ");
            Admin admin = new Admin(string.Empty, string.Empty, 10000);

            Menu._Main(admin);

            User.colored_text("Thanks for playing", ConsoleColor.Blue);
        }

    }

    class Display
    {

        public static void welcome()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine();
            Console.WriteLine("           ###           ########        ###      ##    ##       ####    ##    ##        ########     #######     #######  ");
            Console.WriteLine("          ## ##          ##     ##      ## ##      ##  ##         ##     ###   ##             ##     ##     ##   ##     ## ");
            Console.WriteLine("         ##   ##         ##     ##     ##   ##      ####          ##     ####  ##            ##      ##     ##   ##     ## ");
            Console.WriteLine("        ##     ##        ##     ##    ##     ##      ##           ##     ## ## ##           ##       ##     ##   ##     ## ");
            Console.WriteLine("        #########        ##     ##    #########      ##           ##     ##  ####          ##        ##     ##   ##     ## ");
            Console.WriteLine("        ##     ##        ##     ##    ##     ##      ##           ##     ##   ###         ##         ##     ##   ##     ## ");
            Console.WriteLine("        ##     ##        ########     ##     ##      ##          ####    ##    ##        ########     #######     #######  ");
            Console.WriteLine();
            Console.Beep();
            Console.ResetColor();
        }

        public static void animal(string species)
        {

            switch (species)
            {
                case "Tiger":

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine();
                    Console.WriteLine("           ,''',");
                    Console.WriteLine("         .' ., .',                                  ../'''',");
                    Console.WriteLine("        .'. %%, %.',                            .,/' .,%   :");
                    Console.WriteLine("       .'.% %%%,`%%%'.    .....,,,,,,.....   .,%%% .,%%'. .'");
                    Console.WriteLine("       : %%% %%%%%%',:%%>>%>' .,>>%>>%>%>>%%>,.   `%%%',% :");
                    Console.WriteLine("       : %%%%%%%'.,>>>%'   .,%>%>%'.,>%>%' . `%>>>,. `%%%:'");
                    Console.WriteLine("       ` %%%%'.,>>%'  .,%>>%>%' .,%>%>%' .>>%,. `%%>>,. `%");
                    Console.WriteLine("        `%'.,>>>%'.,%%%%%%%' .,%%>%%>%' >>%%>%>>%.`%% %% `,");
                    Console.WriteLine("        ,`%% %%>>>%%%>>%%>%%>>%>>%>%%%  %%>%%>%%>>%>%%%' % %,");
                    Console.WriteLine("      ,%>%'.>>%>%'%>>%%>%%%%>%'                 `%>%>>%%.`%>>%.");
                    Console.WriteLine("    ,%%>' .>%>%'.%>%>>%%%>>%' ,%%>>%%>%>>%>>%>%%,.`%%%>%%. `%>%.");
                    Console.WriteLine("   ` ,%' .>%%%'.%>%>>%' .,%%%%%%%%'          `%%%%%%.`%%>%% .%%>");
                    Console.WriteLine("   .%>% .%%>' :%>>%%'.,%%%%%%%%%'.%%%%%' `%%%%.`%%%%%.%%%%> %%>%.");
                    Console.WriteLine("  ,%>%' >>%%  >%' `%%%%'     `%%%%%%%'.,>,. `%%%%'     `%%%>>%%>%");
                    Console.WriteLine(".%%>%' .%%>'  %>>%, %% oO ~ Oo %%%>>'.>>>>>>. `% oO ~ Oo'.%%%'%>%,");
                    Console.WriteLine("%>'%> .%>%>%  %%>%%%'  `OoooO'.%%>>'.>>>%>>%>>.`%`OoooO'.%%>% '%>%");
                    Console.WriteLine("%',%' %>%>%'  %>%>%>% .%,>,>,   `>'.>>%>%%>>>%>.`%,>,>' %%%%> .>%>,");
                    Console.WriteLine("` %>% `%>>%%. `%%% %' >%%%%%%>,  ' >>%>>%%%>%>>> >>%%' ,%%>%'.%%>>%.");
                    Console.WriteLine(" .%%'  %%%%>%.   `>%%. %>%%>>>%.>> >>>%>%%%%>%>>.>>>'.>%>%>' %>>%>%%");
                    Console.WriteLine(" `.%%  `%>>%%>    %%>%  %>>>%%%>>'.>%>>>>%%%>>%>>.>',%>>%'  ,>%'>% '");
                    Console.WriteLine("  %>'  %%%%%%'    `%%'  %%%%%> >' >>>>%>>%%>>%>>%> %%>%>' .%>%% .%%");
                    Console.WriteLine(" %>%>, %>%%>>%%,  %>%>% `%%  %>>  >>>%>>>%%>>>>%>>  %%>>,%>%%'.%>%,");
                    Console.WriteLine("%>%>%%, `%>%%>%>%, %>%%> ,%>%>>>.>>`.,.  `\"   ..'>.%. % %>%>%'.%>%%;");
                    Console.WriteLine("%'`%%>%  %%>%%  %>% %'.>%>>%>%%>>%::.  `,   /' ,%>>>%>. >%>%'.%>%'%'");
                    Console.WriteLine("` .%>%'  >%%% %>%%'.>%>%;''.,>>%%>%%::.  ..'.,%>>%>%>,`%  %'.>%%' '");
                    Console.WriteLine("  %>%>%% `%>  >%%'.%%>%>>%>%>%>>>%>%>>%,,::,%>>%%>%>>%>%% `>>%>'");
                    Console.WriteLine("  %'`%%>%>>%  %>'.%>>%>%>>;'' ..,,%>%>%%/::%>%%>>%%,,.``% .%>%%");
                    Console.WriteLine("  `    `%>%>>%%' %>%%>>%>>%>%>%>%%>%/'       `%>%%>%>>%%% ' .%'");
                    Console.WriteLine("        %'  `%>% `%>%%;'' .,>>%>%/',;;;;;,;;;;,`%>%>%,`%'   '");
                    Console.WriteLine("        `    `  ` `%>%%%>%%>%%;/ @a;;;;;;;;;;;a@  >%>%%'");
                    Console.WriteLine("                   `///" + "///" + "///" + "';, `@a@@a@@a@@aa@',;`//'");
                    Console.WriteLine("                      `///" + "///" + ".;;,,............,,;;//'");
                    Console.WriteLine("                          `///" + "/;;;;;;;;;;;;;;;;;/'");
                    Console.WriteLine("                             `///" + "///" + "///" + "///" + "///" + "//'");
                    Console.WriteLine();
                    break;


                case "Lion":

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine();
                    Console.WriteLine("           ,aodObo,\n        ,AMMMMP~~~~");
                    Console.WriteLine("     ,MMMMMMMMA.\n   ,M;'     `YV'\n  AM' ,OMA,");
                    Console.WriteLine(" AM|   `~VMM,.      .,ama,____,amma,..");
                    Console.WriteLine(" MML      )MMMD   .AMMMMMMMMMMMMMMMMMMD.");
                    Console.WriteLine(" VMMM    .AMMY'  ,AMMMMMMMMMMMMMMMMMMMMD");
                    Console.WriteLine(" `VMM, AMMMV'  ,AMMMMMMMMMMMMMMMMMMMMMMM,                ,");
                    Console.WriteLine("  VMMMmMMV'  ,AMY~~''  'MMMMMMMMMMMM' '~~             ,aMM");
                    Console.WriteLine("  `YMMMM'   AMM'        `VMMMMMMMMP'_              A,aMMMM");
                    Console.WriteLine("   AMMM'    VMMA. YVmmmMMMMMMMMMMML MmmmY          MMMMMMM");
                    Console.WriteLine("  ,AMMA   _,HMMMMmdMMMMMMMMMMMMMMMML`VMV'         ,MMMMMMM");
                    Console.WriteLine("  AMMMA _'MMMMMMMMMMMMMMMMMMMMMMMMMMA `'          MMMMMMMM");
                    Console.WriteLine(" ,AMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMa      ,,,   `MMMMMMM");
                    Console.WriteLine(" AMMMMMMMMM'~`YMMMMMMMMMMMMMMMMMMMMMMA    ,AMMV    MMMMMMM");
                    Console.WriteLine(" VMV MMMMMV   `YMMMMMMMMMMMMMMMMMMMMMY   `VMMY'  adMMMMMMM");
                    Console.WriteLine(" `V  MMMM'      `YMMMMMMMV.~~~~~~~~~,aado,`V''   MMMMMMMMM");
                    Console.WriteLine("    aMMMMmv       `YMMMMMMMm,    ,/AMMMMMA,      YMMMMMMMM");
                    Console.WriteLine("    VMMMMM,,v       YMMMMMMMMMo oMMMMMMMMM'    a, YMMMMMMM");
                    Console.WriteLine("    `YMMMMMY'       `YMMMMMMMY' `YMMMMMMMY     MMmMMMMMMMM");
                    Console.WriteLine("     AMMMMM  ,        ~~~~~,aooooa,~~~~~~      MMMMMMMMMMM");
                    Console.WriteLine("       YMMMb,d'         dMMMMMMMMMMMMMD,   a,, AMMMMMMMMMM");
                    Console.WriteLine("        YMMMMM, A       YMMMMMMMMMMMMMY   ,MMMMMMMMMMMMMMM");
                    Console.WriteLine("       AMMMMMMMMM        `~~~~'  `~~~~'   AMMMMMMMMMMMMMMM");
                    Console.WriteLine("       `VMMMMMM'  ,A,                  ,,AMMMMMMMMMMMMMMMM");
                    Console.WriteLine("     ,AMMMMMMMMMMMMMMA,       ,aAMMMMMMMMMMMMMMMMMMMMMMMMM");
                    Console.WriteLine("   ,AMMMMMMMMMMMMMMMMMMA,    AMMMMMMMMMMMMMMMMMMMMMMMMMMMM");
                    Console.WriteLine(" ,AMMMMMMMMMMMMMMMMMMMMMA   AMMMMMMMMMMMMMMMMMMMMMMMMMMMMM");
                    Console.WriteLine("AMMMMMMMMMMMMMMMMMMMMMMMMAaAMMMMMMMMM ");
                    Console.WriteLine();
                    break;

                case "Kangaroo":

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine();
                    Console.WriteLine("                                                                ██████                    ");
                    Console.WriteLine("                                                                ██░░░░██                  ");
                    Console.WriteLine("                                                                ██░░░░░░██████            ");
                    Console.WriteLine("                                                                ██░░░░░░░░░░░░████        ");
                    Console.WriteLine("                                                                  ██░░░░░░░░██░░░░██      ");
                    Console.WriteLine("                                                                  ██░░░░░░░░░░░░░░░░████  ");
                    Console.WriteLine("                                                                  ██▒▒░░░░░░░░░░░░░░▒▒▒▒██");
                    Console.WriteLine("                                                                ██░░░░▒▒▒▒░░░░░░░░▒▒▒▒▒▒██");
                    Console.WriteLine("                                                              ██░░░░░░░░▒▒▒▒▒▒██████████  ");
                    Console.WriteLine("                                              ████████████████░░░░░░░░░░▒▒▒▒██            ");
                    Console.WriteLine("                                      ████████░░░░░░░░░░░░░░░░░░░░░░░░▒▒▒▒██              ");
                    Console.WriteLine("                                    ██░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░▒▒▒▒▒▒██              ");
                    Console.WriteLine("                                  ██░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░▒▒▒▒▒▒██                ");
                    Console.WriteLine("                                ██░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░▒▒██                  ");
                    Console.WriteLine("                              ██░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░██                ");
                    Console.WriteLine("                              ██░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░██                ");
                    Console.WriteLine("                            ██░░░░██░░░░░░░░░░░░░░░░░░░░░░▒▒▒▒▒▒▒▒░░░░░░██                ");
                    Console.WriteLine("                            ██░░██░░░░░░░░░░░░░░████░░░░▒▒▒▒▒▒▒▒▒▒██░░░░████              ");
                    Console.WriteLine("                            ██░░██░░░░░░░░░░░░░░░░██░░▒▒▒▒▒▒▒▒▒▒████▒▒░░░░██              ");
                    Console.WriteLine("                          ██░░░░██▒▒░░░░░░░░░░░░░░██▒▒▒▒▒▒██████  ██▒▒▒▒░░██              ");
                    Console.WriteLine("                          ██░░░░██▒▒▒▒░░░░░░░░░░░░░░██▒▒▒▒██        ██▒▒░░██              ");
                    Console.WriteLine("                          ██░░░░▒▒██▒▒▒▒░░░░░░░░░░░░██████          ██▒▒▒▒██              ");
                    Console.WriteLine("                        ██░░░░░░▒▒██▒▒▒▒▒▒░░░░░░░░░░██              ██▒▒██                ");
                    Console.WriteLine("                        ██░░░░▒▒▒▒██▒▒▒▒▒▒░░░░░░░░██                  ██                  ");
                    Console.WriteLine("                      ██░░░░▒▒▒▒▒▒████▒▒▒▒▒▒░░░░░░██                                      ");
                    Console.WriteLine("                    ██░░░░▒▒▒▒▒▒████████▒▒▒▒░░░░██                                        ");
                    Console.WriteLine("                ████░░░░▒▒▒▒▒▒██  ██████▒▒▒▒▒▒████                                        ");
                    Console.WriteLine("        ████████░░░░░░▒▒▒▒▒▒██      ████▒▒▒▒▒▒██                                          ");
                    Console.WriteLine("  ██████░░░░░░▒▒▒▒▒▒▒▒▒▒▒▒██        ██████▒▒▒▒██                                          ");
                    Console.WriteLine("██░░░░░░░░▒▒▒▒▒▒▒▒▒▒▒▒████          ██████▒▒▒▒░░██████████                                ");
                    Console.WriteLine("██████████████████████              ██████████▒▒▒▒▒▒░░░░░░██                              ");
                    Console.WriteLine("                                      ██████████████████████                              ");
                    Console.WriteLine();
                    break;

                case "Penguin":

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine();
                    Console.WriteLine("                ________o8A888888o_");
                    Console.WriteLine("            _o888888888888K_]888888o");
                    Console.WriteLine("                      ~~~+8888888888o");
                    Console.WriteLine("                          ~8888888888");
                    Console.WriteLine("                          o88888888888");
                    Console.WriteLine("                         o8888888888888");
                    Console.WriteLine("                       _8888888888888888");
                    Console.WriteLine("                      o888888888888888888_");
                    Console.WriteLine("                     o88888888888888888888_");
                    Console.WriteLine("                    _8888888888888888888888_");
                    Console.WriteLine("                    888888888888888888888888_");
                    Console.WriteLine("                    8888888888888888888888888");
                    Console.WriteLine("                    88888888888888888888888888");
                    Console.WriteLine("                    88888888888888888888888888");
                    Console.WriteLine("                    888888888888888888888888888");
                    Console.WriteLine("                    ~88888888888888888888888888_");
                    Console.WriteLine("                     (88888888888888888888888888");
                    Console.WriteLine("                      888888888888888888888888888");
                    Console.WriteLine("                       888888888888888888888888888_");
                    Console.WriteLine("                       ~8888888888888888888888888888");
                    Console.WriteLine("                         +88888888888888888888~~~~~");
                    Console.WriteLine("                          ~=888888888888888888o");
                    Console.WriteLine("                   _=oooooooo888888888888888888");
                    Console.WriteLine("                    _o88=8888==~88888888===8888_");
                    Console.WriteLine("                    ~   =~~ _o88888888=      ~~~");
                    Console.WriteLine("                            ~ o8=~88=~");
                    Console.WriteLine();
                    break;

            }

            Console.Beep(659, 200);
            Console.Beep(900, 500);
            Console.ResetColor();
        }

    }
}
