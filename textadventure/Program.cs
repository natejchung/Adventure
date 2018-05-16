using System;
using System.Timers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace textadventure
{

    
    class Program

    {
        static room[] rooms;
        static int curLoc = 0;
        class person
        {
            public string name;
            public int age;
            public string description;
            public string speech;
            public string speechtwo;
            public string speechthree;
            public bool anger;
            public int hasspoken;
            public person(string n, int a, string d, string s, string st, string sth, bool an)
            {
                name = n;
                age = a;
                description = d;
                speech = s;
                speechtwo = st;
                speechthree = sth;
                anger = an;
            }
        }

        class weapon: item
        {
            public weapon(string n, bool i, bool iv) : base(n, i, iv)
            {
            }

        }

        class healing : item
        {
            public healing(string n, bool i, bool iv) : base(n, i, iv)
            {
            }

        }
        class Ingredient : item
        {
            public Ingredient(string n, bool i, bool iv) : base(n, i, iv)
            {
            }
        }
        class Recipe
        {
            public List<Ingredient> ingredients;
            public string name;
            public Recipe(string n, List<Ingredient> ing)
            {
                name = n;
                ingredients = ing;
            }
        }
        //class sandwich
        //{
        //    public string veggie;
        //    public string bread;
        //    public string meat;
        //    public string cheese;


        //}
        class key : item
        {
            public int doorroom;
            public int nextroom;
            public string direction;

            public key(string n, int d, int r, string i, bool infinite, bool iv) : base(n, infinite, iv)
            {

                doorroom = d;
                nextroom = r;
                direction = i;

            }
            public override void use()
            {
                switch (direction)
                {
                    case "N":
                        rooms[doorroom].N = nextroom;
                        break;

                    case "S":
                        rooms[doorroom].S = nextroom;
                        break;

                    case "W":
                        rooms[doorroom].W = nextroom;
                        break;

                    case "E":
                        rooms[doorroom].E = nextroom;
                        break;

                    default:
                        Console.WriteLine("Woops");

                        break;
                }


            }   
        }



        struct room
        {
            public string name;
            public string shortDescription;
            public string longDescription;
            public bool visited;
            public int N;
            public int S;
            public int E;
            public int W;
            public List<item> items;
            public List<person> person;
        }
        public static void setupRooms()
        {
            StreamReader reader = new StreamReader("./map.csv");
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                string[] parameters = line.Split(',');
                room r = new room();
                r.person = new List<person>();
                r.items = new List<item>();
                for (int index = 1; index < parameters.Length; index++)
                {
                    switch (index)
                    {
                        case 1:
                            r.name = parameters[index];
                            break;
                        case 2:
                            r.longDescription = parameters[index];
                            break;
                        case 3:
                            r.shortDescription = parameters[index];
                            break;
                        case 4:
                            r.visited = parameters[index] == "1" ? true:false;
                            break;
                        case 5:
                            r.N = int.Parse(parameters[index]);
                            break;
                        case 6:
                            r.S = int.Parse(parameters[index]);
                            break;
                        case 7:
                            r.E = int.Parse(parameters[index]);
                            break;
                        case 8:
                            r.W = int.Parse(parameters[index]);
                            break;
                    }
                }
                rooms[int.Parse(parameters[0])] = r;
            }
        }
        private static void WriteSlowly(string text)
        {
            string[] words = text.Split(' ');
            Task t = Task.Run(() =>
            {
                foreach (string word in words)
                {
                    foreach (char letter in word)
                    {
                        Console.Write(letter);
                        System.Threading.Thread.Sleep(50);
                    }
                    Console.Write(" ");
                    System.Threading.Thread.Sleep(10);
                }
            });
            t.Wait();
        }
        public static int health = 100;
        //public static Timer timer = new Timer();
        //timer.Interval = 1000;
            //timer.Elapsed += Timer_Elapsed;
        static void Main(string[] args)
        {
            Ingredient bread = new Ingredient("BREAD", false, false);
            Ingredient chicken = new Ingredient("CHICKEN", false,false );
            item SANDWICH = new item("SANDWICH", false, false);
            Recipe[] recipes = new Recipe[] { new Recipe("SANDWICH", new List<Ingredient>(new Ingredient[] { bread, chicken })) };
            

            WriteSlowly("Welcome to the text adventure");
            Console.Clear();
          // WriteSlowly("You have woken up and you have no idea where you are.  Type n to go north, s for south, w for west, and e for east.Type (take) and name of an item to pick it up.  Type (use) and then the items name to use it.  To view inventory, type the word (Inventory).  Type (help) to get help.  You are losing health as you are walking around, so be sure to eat something every so often.");
            Console.WriteLine("Type PLAY to continue.");
            string startresponse = Console.ReadLine();
            bool gameOver = false;
            if (startresponse.ToUpper() == "PLAY")
            {
                do
                {
                    gameOver = false;
                    curLoc = 0;

                    rooms = new room[100];
                    for (int i = 0; i < rooms.Length; i++)
                    {
                        rooms[i].person = new List<person>();
                        rooms[i].items = new List<item>();
                    }

                    //rooms[0].name = "Closet";
                    //rooms[0].longDescription = "A closet with biology experiments";
                    //rooms[0].shortDescription = "A dark dimly lit closet with a musty smell";
                    //rooms[0].N = 1;
                    //rooms[0].S = -1;
                    //rooms[0].E = -1;
                    //rooms[0].W = -1;
                    //rooms[0].visited = false;
                    setupRooms();
                    rooms[1].name = "Laboratory";
                    rooms[1].longDescription = "a classroom with chairs and tables.   There is a table with one chair with a sink at the front.  A man is sitting in it.";
                    rooms[1].shortDescription = "A small classroom with tables and chairs.  There is a man in the room.";
                    rooms[1].N = -1;
                    rooms[1].S = 0;
                    rooms[1].E = 2;
                    rooms[1].W = -1;
                    rooms[1].person.Add(new person("JEFF", 21, "A tall male in a lab white coat, slightly nerdy looking", "Why Hello there!  Who are you?  You don't know?  How unusual... well maybe we can figure this out together.  Let's write down what we know...Oh my!  I don't have any paper or pencil.  Could you go get some?  You'll probably find some in my office.", "Hello again.  Thanks for helping me!  ", "Hello my friend", false));
                    rooms[2].name = "Hallway";
                    rooms[2].longDescription = "The posters are all research projects of Jeffery.  He is very smart.";
                    rooms[2].shortDescription = "A hallway with science posters on the walls";
                    rooms[2].N = 4;
                    rooms[2].S = 3;
                    rooms[2].E = -1;
                    rooms[2].W = 1;
                    rooms[3].name = "Lobby";
                    rooms[3].longDescription = "hi";
                    rooms[3].shortDescription = "A large room with some couches";
                    rooms[3].N = 2;
                    rooms[3].S = -1;
                    rooms[3].E = 6;
                    rooms[3].W = 5;
                    rooms[3].items.Add(new item("PIZZA", false, false));
                    rooms[3].items.Add(new item("SODA", false, false));
                    rooms[4].name = "Sunlit room";
                    rooms[4].longDescription = "through the windows you see a field and beyond that, buildings.  Since there are no exists you might have to break a window.";
                    rooms[4].shortDescription = "A room with windows";
                    rooms[4].N = -1;
                    rooms[4].S = 2;
                    rooms[4].E = -1;
                    rooms[4].W = -1;                   
                    rooms[5].name = "An office";
                    rooms[5].longDescription = "There is a picture of Jeff on the desk";
                    rooms[5].shortDescription = "A room with shelves holding books, paper, pencil and erasers.";
                    rooms[5].N = -1;
                    rooms[5].S = -1;
                    rooms[5].E = 3;
                    rooms[5].W = -1;
                    rooms[5].items.Add(new key("BATON", 4, 9, "N", false,false));
                    rooms[5].items.Add(new item("BOOKS", false, false));
                    rooms[5].items.Add(new item("PAPER", false,false));
                    rooms[5].items.Add(new item("PENCIL", false,false));
                    rooms[6].name = "Stairs";
                    rooms[6].longDescription = "Just an average staircase in a college.";
                    rooms[6].shortDescription = "Some dark steps leading down to a pair of metal doors";
                    rooms[6].N = -1;
                    rooms[6].S = 7;
                    rooms[6].E = -1;
                    rooms[6].W = 3;
                    rooms[6].items.Add(new item("SHOVEL", false,false));
                    rooms[7].name = "path";
                    rooms[7].longDescription = "";
                    rooms[7].shortDescription = "You are on a concrete path with metal fences on either side";
                    rooms[7].N = 6;
                    rooms[7].S = -1;
                    rooms[7].E = -1;
                    rooms[7].W = -1;
                    rooms[9].name = "Grass and Trees";
                    rooms[9].longDescription = "";
                    rooms[9].N = 10;
                    rooms[9].S = 4;
                    rooms[9].E = 12;
                    rooms[9].W = -1;
                    rooms[10].name = "Music Building";
                    rooms[10].shortDescription = "a building to practice/perform instruments";
                    rooms[10].longDescription = "a large modern looking building.  Inside there are studios that are locked.  Probably because of the expensive music equipment";
                    rooms[10].N = 11;
                    rooms[10].S = 9;
                    rooms[10].E = -1;
                    rooms[10].W = -1;
                    rooms[11].name = "Stage";
                    rooms[11].longDescription = "A large music performance platform.  There is a piano on the stage.";
                    rooms[11].shortDescription = "A music performance platform.";
                    rooms[11].N = 15;
                    rooms[11].S = 10;
                    rooms[11].E = -1;
                    rooms[11].W = -1;
                    rooms[12].name = "church";
                    rooms[12].longDescription = "A cathedral with stained glass windows.  A large Pipe Organ lies at the front of the hall";
                    rooms[12].shortDescription = "A church, with uncomfortable wooden pews.";
                    rooms[12].N = -1;
                    rooms[12].S = 30;
                    rooms[12].E = 13;
                    rooms[12].W = 9;
                    rooms[13].name = "Athletic Field";
                    rooms[13].longDescription = "a football field surrounded by a track with various athletic equipment along the sides";
                    rooms[13].shortDescription = "an outdoor sports area.";
                    rooms[13].N = -1;
                    rooms[13].S = 14;
                    rooms[13].E = -1;
                    rooms[13].W = 12;
                    rooms[14].name = "University Center";
                    rooms[14].shortDescription = "a dormatory for the cool kids on campus";
                    rooms[14].longDescription = "A dorm that has turned into a hub for socialization on campus.  There are many games and activities here.  Also usually there are people here, but its empty now.";
                    rooms[14].N = 13;
                    rooms[14].S = 24;
                    rooms[14].E = -1;
                    rooms[14].W = -1;
                    rooms[15].name = "Piano";
                    rooms[15].shortDescription = "a beautiful instrument ... ";
                    rooms[15].longDescription = "The greatest of instruments, competing only with a the human voice ";
                    rooms[15].N = -1;
                    rooms[15].S = 11;
                    rooms[15].E = -1;
                    rooms[15].W = -1;
                    rooms[15].items.Add(new key("KEY", 7, 16, "S", false,true));
                    rooms[16].name = "bookstore";
                    rooms[16].shortDescription = "The name can be deceiving.   The bookstore is the place to be.";
                    rooms[16].longDescription = "A fun place with snacks and books.  Clothing, accessories, and various items can be bought here to.  Students often purchase textbooks here as well.";
                    rooms[16].N = 7;
                    rooms[16].S = 19;
                    rooms[16].E = -1;
                    rooms[16].W = -1;
                    rooms[17].name = "pool";
                    rooms[17].shortDescription = "A swimming pool.  The Swim team practices here.";
                    rooms[17].longDescription = "An olympic sized swimming pool.  Very clear and very cold water.";
                    rooms[17].N = -1;
                    rooms[17].S = 18;
                    rooms[17].E = -1;
                    rooms[17].W = -1;
                    rooms[18].name = "gymnastics center";
                    rooms[18].shortDescription = "a fun place where top athletes train in gymnastics and anyone else can come practice gymnastics";
                    rooms[18].longDescription = "There are many trampolines, rings, pommel horses and other gymnastic equipment here.  There are also foam pits and rock climbing pits.  People often have birthday parties here.";
                    rooms[18].N = 17;
                    rooms[18].S = 21;
                    rooms[18].E = -1;
                    rooms[18].W = 19;
                    rooms[19].name = "parking lot";
                    rooms[19].shortDescription = "a parking lot with cars. ";
                    rooms[19].longDescription = "this is the main parking lot for campus";
                    rooms[19].N = -1;
                    rooms[19].S = -1;
                    rooms[19].E = 18;
                    rooms[19].W = 20;
                    rooms[20].name = "art department";
                    rooms[20].shortDescription = "Want to become a painter?";
                    rooms[20].longDescription = "Painting, sculpting and everything else art.  I don't know much about this building";
                    rooms[20].N = -1;
                    rooms[20].S = 22;
                    rooms[20].E = 19;
                    rooms[20].W = -1;
                    rooms[21].name = "gymnastics";
                    rooms[21].shortDescription = "Here is where the gymnastic team practices";
                    rooms[21].longDescription = "Many ridiculously muscled men and women fliping about and performing mind-boggling feats of strength.";
                    rooms[21].N = 18;
                    rooms[21].S = -1;
                    rooms[21].E = -1;
                    rooms[21].W = -1;
                    rooms[22].name = "impressionist garden";
                    rooms[22].shortDescription = "not a garden of foods, which is more practical.";
                    rooms[22].longDescription = "This was a project of one class from the art deparment.  It hosts many intricate beautiful designs created from natural and often living things.";
                    rooms[22].N = 20;
                    rooms[22].S = 1;
                    rooms[22].E = 1;
                    rooms[22].W = 2;
                    rooms[23].name = "river";
                    rooms[23].shortDescription = "a wide, flowing body of water.";
                    rooms[23].longDescription = "There are people fishing in the river.  The water is surprisingly clear, and not too deep.  One could probably cross it if needed to.";
                    rooms[23].N = 2;
                    rooms[23].S = 1;
                    rooms[23].E = 1;
                    rooms[23].W = 2;
                    rooms[24].name = "shrubs";
                    rooms[24].shortDescription = "";
                    rooms[24].longDescription = "";
                    rooms[24].N = 14;
                    rooms[24].S = -1;
                    rooms[24].E = 25;
                    rooms[24].W = -1;
                    rooms[25].name = "trees";
                    rooms[25].shortDescription = "";
                    rooms[25].longDescription = "";
                    rooms[25].N = -1;
                    rooms[25].S = -1;
                    rooms[25].E = 26;
                    rooms[25].W = 24;
                    rooms[26].name = "river";
                    rooms[26].shortDescription = "the river is narrowing ";
                    rooms[26].longDescription = "the part of the river that begins to narrow";
                    rooms[26].N = -1;
                    rooms[26].S = -1;
                    rooms[26].E = 27;
                    rooms[26].W = -1;
                    rooms[26].items.Add(new key("FRESHWATER", 31 , 32 , "E" , true, false));
                    rooms[27].name = "river continued";
                    rooms[27].shortDescription = "";
                    rooms[27].longDescription = "";
                    rooms[27].N = 28;
                    rooms[27].S = -1;
                    rooms[27].E = -1;
                    rooms[27].W = 26;
                    rooms[27].items.Add(new key("FRESHWATER", 31 , 33 , "S" , true, false));
                    rooms[28].name = "house";
                    rooms[28].shortDescription = "";
                    rooms[28].longDescription = "";
                    rooms[28].N = 2;
                    rooms[28].S = 1;
                    rooms[28].E = 1;
                    rooms[28].W = 2;
                    rooms[28].person.Add(new person("BETHANY", 21, "", "", "", "", false));
                    rooms[36].name = "road";
                    rooms[36].longDescription = "";
                    rooms[36].N = 2;
                    rooms[36].S = 1;
                    rooms[36].E = 1;
                    rooms[36].W = 2;
                    rooms[36].person.Add(new person("HARRY", 21, "Sup Bruh.", "", "", "", false));
                    rooms[37].name = "Mansion";
                    rooms[37].longDescription = "";
                    rooms[37].N = 2;
                    rooms[37].S = 1;
                    rooms[37].E = 1;
                    rooms[37].W = 2;
                    rooms[37].person.Add(new person("BUTLER", 21, "Good Morning, sir.", "", "", "", false));
                    rooms[34].name = "beach";
                    rooms[34].longDescription = "sand...sand..";
                    rooms[34].longDescription = "sand.  It's a beach.";
                    rooms[34].N = 2;
                    rooms[34].S = 1;
                    rooms[34].E = 1;
                    rooms[34].W = 2;
                    rooms[35].name = "beach";
                    rooms[35].longDescription = "lots of sand here...";
                    rooms[35].shortDescription = "sand, sand, and more sand.  Wanna build a castle?";
                    rooms[35].N = 2;
                    rooms[35].S = 1;
                    rooms[35].E = 1;
                    rooms[35].W = 2;
                    rooms[29].name = "ocean";
                    rooms[29].longDescription = "an endless expansion of saltwater";
                    rooms[29].longDescription = "a beach on an ocean means lots of activity.  Strangely there are not many here.  Only a few hardcore surfers tearing the waves.";
                    rooms[29].N = 2;
                    rooms[29].S = 1;
                    rooms[29].E = 1;
                    rooms[29].W = 2;
                    rooms[36].person.Add(new person("DERRICK", 21, "Sup Bruh.  The waves are worth shredding today.", "", "", "", false));
                    rooms[29].items.Add(new item("SALTWATER", true, false));
                    rooms[30].name = "Cafeteria";
                    rooms[30].longDescription = "The Cafeteria is unusually empty.";
                    rooms[30].shortDescription = "The best place on campus.";
                    rooms[30].N = 12;
                    rooms[30].S = -1;
                    rooms[30].E = 31;
                    rooms[30].W = -1;
                    rooms[36].person.Add(new person("DORRIS", 21, "Hello.  There's a lot to clean here", "", "", "", false));
                    rooms[31].name = "Kitchen";
                    rooms[31].longDescription = "There is fridge";
                    rooms[31].shortDescription = "just like every other kitchen..";
                    rooms[31].N = -1;
                    rooms[31].S = -1;
                    rooms[31].E = -1;
                    rooms[31].W = 30;
                    rooms[31].person.Add(new person("ALFRED", 21, "An overweight man with a meat clever and a huge white hat.", "Who are you? what are you doing in my kitchen?  Nobody can enter my kitchen ", "I apologize for attacking you.  You must understand that people always attempt to steal food.  I'll give you access to the kitchen if you find me some water.", "take what you want", true));
                    rooms[32].name = "Refrigerator";
                    rooms[32].shortDescription = "a huge industrial fridge";
                    rooms[32].longDescription = "food is here";
                    rooms[32].N = -1;
                    rooms[32].S = -1;
                    rooms[32].E = -1;
                    rooms[32].W = 31;
                    rooms[33].name = "Pantry";
                    rooms[33].longDescription = "food";
                    rooms[33].N = 31;
                    rooms[33].S = -1;
                    rooms[33].E = -1;
                    rooms[33].W = -1;
                    rooms[33].items.Add(bread);
                    rooms[33].items.Add(chicken);
                    rooms[33].items.Add(new Ingredient("SPICES", true, false));
                    rooms[38].name = "DOCKS";
                    rooms[38].longDescription = "";
                    rooms[38].N = 2;
                    rooms[38].S = 1;
                    rooms[38].E = 1;
                    rooms[38].W = 2;
                    rooms[39].name = "DAIRYFARM";
                    rooms[39].longDescription = "";
                    rooms[39].N = 2;
                    rooms[39].S = 1;
                    rooms[39].E = 1;
                    rooms[39].W = 2;
                    rooms[40].name = "WALKINGPATH";
                    rooms[40].longDescription = "";
                    rooms[40].N = 2;
                    rooms[40].S = 1;
                    rooms[40].E = 1;
                    rooms[40].W = 2;
                    rooms[41].name = "BOAT";
                    rooms[41].shortDescription = "A large yacht.";
                    rooms[41].longDescription = "It would appear the people who fund the university are touring it and they arrived in their boat.";
                    rooms[41].N = 2;
                    rooms[41].S = 1;
                    rooms[41].E = 1;
                    rooms[41].W = 2;
                    rooms[42].name = "PARKBENCH";
                    rooms[42].shortDescription = "an old man sits on the bench";
                    rooms[42].longDescription = "Looks as if he's playing a game.";
                    rooms[42].N = 2;
                    rooms[42].S = 1;
                    rooms[42].E = 1;
                    rooms[42].W = 2;
                    rooms[43].name = "PARK";
                    rooms[43].longDescription = "";
                    rooms[43].N = 2;
                    rooms[43].S = 1;
                    rooms[43].E = 1;
                    rooms[43].W = 2;
                    rooms[43].name = "PRODUCTIONSTUDIO";
                    rooms[44].longDescription = "";
                    rooms[44].N = 2;
                    rooms[44].S = 1;
                    rooms[44].E = 1;
                    rooms[44].W = 2;
                    rooms[45].name = "COMPUTERLAB";
                    rooms[45].shortDescription = "the nerd hub";
                    rooms[45].longDescription = "if you would like to learn to code this is the place to be.  From video games to webpages.";
                    rooms[45].N = 2;
                    rooms[45].S = 1;
                    rooms[45].E = 1;
                    rooms[45].W = 2;
                    rooms[46].name = "DORMATORY";
                    rooms[46].longDescription = "";
                    rooms[46].N = 2;
                    rooms[46].S = 1;
                    rooms[46].E = 1;
                    rooms[46].W = 2;
                    rooms[47].name = "LIBRARY";
                    rooms[47].longDescription = "";
                    rooms[47].N = 2;
                    rooms[47].S = 1;
                    rooms[47].E = 1;
                    rooms[47].W = 1;
                    rooms[48].name = "REGISTRATION";
                    rooms[48].longDescription = "";
                    rooms[48].N = 2;
                    rooms[48].S = 1;
                    rooms[48].E = 1;
                    rooms[48].W = 1;
                    rooms[49].name = "CAMPUSSAFETY";
                    rooms[49].shortDescription = "the security of campus";
                    rooms[49].longDescription = "Any problems can be brought to the people here.  There is a very large man who is looking at you like you are a criminal";
                    rooms[49].N = 2;
                    rooms[49].S = 1;
                    rooms[49].E = 1;
                    rooms[49].W = 1;
                    rooms[36].person.Add(new person("JUSTIN", 21, "Why hello there young man who are you?", "", "", "", true));
                    rooms[50].name = "DEPARTMENTOFSCIENCES";
                    rooms[50].longDescription = "";
                    rooms[50].N = 2;
                    rooms[50].S = 1;
                    rooms[50].E = 1;
                    rooms[50].W = 1;
                    //END OF ROOMS CODE
                    List<item> myItems = new List<item>();
                    List<item> edibleItems = new List<item>();

                    string command = "";
                    string message = "";
                    do
                    {
                        if (gameOver)
                        {
                            break;
                        }
                        Console.Clear();
                        Console.WriteLine("You are in the " + rooms[curLoc].name);
                        if (rooms[curLoc].visited == false)
                        {
                            Console.WriteLine(rooms[curLoc].longDescription);
                            rooms[curLoc].visited = true;
                        }
                        else
                        {
                            Console.WriteLine(rooms[curLoc].shortDescription);
                        }
                        Console.WriteLine("Obvious exits are:");
                        if (rooms[curLoc].N >= 0)
                        {
                            Console.Write("N ");
                        }
                        if (rooms[curLoc].S >= 0)
                        {
                            Console.Write("S ");
                        }
                        if (rooms[curLoc].E >= 0)
                        {
                            Console.Write("E ");
                        }
                        if (rooms[curLoc].W >= 0)
                        {
                            Console.Write("W ");
                        }

                        Console.WriteLine();
                        Console.WriteLine("Visible Items: ");
                        for (int i = 0; i < rooms[curLoc].items.Count; i++)
                        {
                            if (!rooms[curLoc].items[i].invisible)
                            {
                                Console.Write(rooms[curLoc].items[i].name + "");
                            }
                        }
                        if (rooms[curLoc].items.Count == 0)
                        {
                            Console.Write("None");
                        }
                     
                        Console.WriteLine();

                        if (message != "")
                        {
                            Console.WriteLine(message);
                            message = "";
                        }
                        //People
                        Console.WriteLine("Persons visible:");
                        for (int i = 0; i < rooms[curLoc].person.Count; i++)
                        {
                            Console.Write(rooms[curLoc].person[i].name + "");
                        }
                        if (rooms[curLoc].person.Count == 0)
                        {
                            Console.Write("Nobody here");
                        }
                        Console.WriteLine();

                        if (message != "")
                        {
                            Console.WriteLine(message);
                            message = "";
                        }
                        Console.WriteLine("\nCommand?");
                        command = Console.ReadLine();
                        //command = command.ToUpper();
                        if (health == 0)
                        {
                            Console.Clear();
                            Console.WriteLine("You Died.  You let your health go too low.");
                            Console.WriteLine("Would you like to restart(type yes or no)?");
                            string restart = Console.ReadLine();
                            if (restart.ToUpper() == "YES")
                            {
                                gameOver = true;
                                continue;
                            } else if(restart.ToUpper() == "NO"){
                                Console.Clear();
                                Console.WriteLine("Thanks for playing");
                            }

                        }
                        
                        string[] commands = command.ToUpper().Split(' ');
                        if (commands.Length == 1)
                        {
                            for (int i = 0; i < rooms[curLoc].items.Count; i++)
                            {
                                Console.Write(rooms[curLoc].items[i] + " ");
                            }
                            if (rooms[curLoc].items.Count == 0)
                                Console.Write("NONE");
                            Console.WriteLine();
                            if (commands[0] == "N" && rooms[curLoc].N >= 0)
                            {
                                curLoc = rooms[curLoc].N;
                                health -= 1;
                                Console.WriteLine(health);
                            }
                            else if (commands[0] == "S" && rooms[curLoc].S >= 0)
                            {
                                curLoc = rooms[curLoc].S;
                                health -= 1;
                                Console.WriteLine(health);
                            }
                            else if (commands[0] == "E" && rooms[curLoc].E >= 0)
                            {
                                curLoc = rooms[curLoc].E;
                                health -= 1;
                                Console.WriteLine(health);
                            }
                            else if (commands[0] == "W" && rooms[curLoc].W >= 0)
                            {
                                curLoc = rooms[curLoc].W;
                                health -= 1;
                                Console.WriteLine(health);
                            }
                            else if (commands[0] == "TAKE")
                            {
                                message = "You have to say what you want to take.";
                            }
                            else if (commands[0] == "INVENT")
                            {
                            }
                            else if (commands[0] == "LONG")
                            {
                                Console.WriteLine(rooms[curLoc].longDescription);
                                Console.ReadLine();
                            }
                            else if (commands[0] == "INVENTORY")
                            {
                                message = "Inventory consists of:";
                                if (myItems.Count > 0)
                                {
                                    foreach (item item in myItems)
                                    {
                                        message += "\n" + item.name;
                                    }
                                }
                                else
                                {
                                    message = "There is nothing in your inventory";
                                }
                            }

                            else if (commands[0] == "HELP")
                            {
                                message = "Your goal is to save the University. To move throughout the map, use first letter of the 4 directions: N,S,W, and E. If there are items in the room, you can take them. Inventory displays current inventory.";
                                //for (int i = 0; i < help.Count(); i++)
                                //{
                                //    message += "\n" + help[i];
                                //}
                            }
                            else
                                message = "You can't go that way";
                        }
                        else if (commands.Length == 2)
                        {
                            if (commands[0] == "TAKE")
                            {
                                foreach (item item in rooms[curLoc].items)
                                {
                                    if (item.name == commands[1] && item.invisible == false && myItems.Count <= 7)
                                    {
                                        if (item.infinite == false)
                                        {
                                            myItems.Add(item);
                                            rooms[curLoc].items.Remove(item);
                                            break;
                                        }
                                        else if (item.infinite == true)
                                        {
                                            myItems.Add(item);
                                            break;
                                        }
                                    } else if (item.name == commands[1] && item.invisible == false && myItems.Count > 7)
                                    {
                                       
                                        Console.WriteLine("You are carrying too many things.");
                                    }
                                }
                            }
                        
                        else if (commands[0] == "DROP")
                            {
                                foreach (item item in rooms[curLoc].items)
                                {
                                    if(item.name == commands[1])
                                    {
                                        myItems.Remove(item);
                                        rooms[curLoc].items.Add(item);
                                    }
                                }
                               
                            }

                        else if (commands[0] == "USE")
                        {
                            foreach (item item in myItems)
                            {
                                if (item.name == commands[1])
                                {
                                    item.use();

                                    break;
                                }
                            }
                        }
                        else if (commands[0] == "MAKE" && commands[1] == "SANDWICH")
                        {
                            int numIngredients = recipes[0].ingredients.Count();
                            List<Ingredient> found = new List<Ingredient>();
                            bool isFound;
                            foreach (Ingredient ingredient in recipes[0].ingredients)
                            {
                                isFound = false;
                                foreach (item i in myItems)
                                {
                                    if (i == ingredient && i.GetType() == new Ingredient("", true, false).GetType())
                                    {
                                        found.Add((Ingredient)i);
                                        isFound = true;
                                        break;
                                    }
                                }
                                if (!isFound)
                                {
                                    break;
                                }
                            }

                            if (found.Count == numIngredients)
                            {
                                foreach (Ingredient i in found)
                                {
                                    myItems.Remove(i);
                                }
                                myItems.Add(SANDWICH);
                            }
                        }
                        else if (commands[0] == "LOOK" && commands[1] == "CLOSER")
                        {

                            foreach (item item in rooms[curLoc].items)
                            {

                                if (item.invisible == false)
                                {


                                }
                                else if (item.invisible == true)
                                {
                                    item.invisible = false;
                                }
                            }
                        }
                        }
                        else if (commands.Length == 3)
                        {
                            if (commands[0] == "SPEAK" && commands[1] == "TO")
                            {
                                foreach (person person in rooms[curLoc].person)
                                {
                                    if (person.name == commands[2])
                                    {
                                        if (person.hasspoken == 0)
                                        {
                                            WriteSlowly(person.speech);
                                            Console.ReadLine();
                                            person.hasspoken = 1;
                                            if (person.anger == false)
                                            {
                                            }
                                            else if (person.anger == true)
                                            {
                                                while (person.anger == true && health > 0 )
                                                {
                                                    
                                                    //timer.Start();
                                                    Console.WriteLine("You are being attacked!");
                                                    Console.WriteLine("Would You like to defend or run?  (Type 'defend' or 'run')");
                                                    if (Console.ReadLine() == "defend")
                                                    {

                                                        do
                                                        {
                                                            Console.WriteLine("Do you choose rock,paper or scissors");
                                                            string userChoice = Console.ReadLine();                                                    
                                                            Random z = new Random();
                                                            int computerChoice = z.Next(4);

                                                            if (computerChoice == 1)
                                                            {
                                                                if (userChoice == "rock")
                                                                {
                                                                    Console.WriteLine(person.name + " chose rock");
                                                                    Console.WriteLine("It is a tie ");
                                                                }
                                                                else if (userChoice == "paper")
                                                                {
                                                                    Console.WriteLine("The computer chose paper");
                                                                    Console.WriteLine("It is a tie ");

                                                                }
                                                                else if (userChoice == "scissors")
                                                                {
                                                                    Console.WriteLine("The computer chose scissors");
                                                                    Console.WriteLine("It is a tie ");
                                                                }
                                                                else
                                                                {
                                                                    Console.WriteLine("You must choose rock, paper or scissors!");
                                                                }
                                                            }
                                                            else if (computerChoice == 2)
                                                            {
                                                                if (userChoice == "rock")
                                                                {
                                                                    Console.WriteLine("The computer chose paper");
                                                                    Console.WriteLine("Sorry you lose,paper beat rock");
                                                                    health -= 20;
                                                                    Console.WriteLine(health);
                                                                }
                                                                else if (userChoice == "paper")
                                                                {
                                                                    Console.WriteLine("The computer chose scissors");
                                                                    Console.WriteLine("Sorry you lose,scissors beat paper ");
                                                                    health -= 20;
                                                                    Console.WriteLine(health);
                                                                }
                                                                else if (userChoice == "scissors")
                                                                {
                                                                    Console.WriteLine("The computer chose rock");
                                                                    Console.WriteLine("Sorry you lose,rock beats scissors");
                                                                    health -= 20;
                                                                    Console.WriteLine(health);
                                                                }
                                                                else
                                                                {
                                                                    Console.WriteLine("You must choose rock,paper or scissors!");
                                                                }
                                                            }
                                                            else if (computerChoice == 3)
                                                            {
                                                                if (userChoice == "rock")
                                                                {
                                                                    Console.WriteLine("The computer chose scissors");
                                                                    Console.WriteLine("You win,rock beats scissors");

                                                                }
                                                                else if (userChoice == "paper")
                                                                {
                                                                    Console.WriteLine("The computer chose rock");
                                                                    Console.WriteLine("You win,paper beats rock");

                                                                }
                                                                else if (userChoice == "scissors")
                                                                {
                                                                    Console.WriteLine("The computer chose paper");
                                                                    Console.WriteLine("You win,scissors beat paper");
                                                                }
                                                                else
                                                                {
                                                                    Console.WriteLine("You must choose rock,paper or scissors!");

                                                                }
                                                            }
                                                            Console.ReadLine();
                                                        } while (Console.ReadLine() == "yes");                 
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                        else if (person.hasspoken == 1)
                                        {
                                            WriteSlowly(person.speechtwo);
                                            Console.ReadLine();
                                            person.hasspoken = 2;
                                            break;
                                        }
                                        else if (person.hasspoken == 2)
                                        {
                                            WriteSlowly(person.speechtwo);
                                            Console.ReadLine();
                                            person.hasspoken = 3;
                                            break;
                                        }


                                    }
                                }
                            }
                            else if (commands[0] == "DEFEND" && commands[1] == "AGAINST")
                            {
                                foreach (person person in rooms[curLoc].person)
                                {
                                    if (person.name == commands[2])
                                    { 
                                    }
                                }
                            }
                        }
                        
                        //else if (commands[0] == "EAT" && commands[1] == "SANDWICH")
                        //{
                        //    myItems.Remove(SANDWICH);
                        //    health += 20;
                        //}
                        //else if (commands[0] == "GIVE" && commands[1] == "SANDWICH")
                        //{
                        //    myItems.Remove(item(SANDWICH));
                        //}
                        //else if (commands[0] == "EAT")
                        //{
                        //    foreach (item item in myItems)
                        //    {
                        //        if (item.name == commands[1])
                        //        {
                        //            item.use();

                        //            break;
                        //        }
                        //    }
                        //}
                        else
                        {
                            message = "Please retype command!";
                        }
                    } while (command != "Q" && command != "QUIT");

                } while (startresponse != "NO" && startresponse != "QUIT");
            }
        }
        //private static void Timer_Elapsed(object sender, ElapsedEventArgs e)
        //{
        //    health -= 20;
        //}
    }
}
