using System;
using System.Linq;
using System.Text;

namespace PipServices.Commons.Random
{
    public class RandomText
    {
        private static readonly string[] _namePrefixes = new string[] { "Dr.", "Mr.", "Mrs" };
        private static readonly string[] _nameSuffixes = new string[] { "Jr.", "Sr.", "II", "III" };
        private static readonly string[] _firstNames = new string[] {
            "John", "Bill", "Andrew", "Nick", "Pamela", "Bela", "Sergio", "George", "Hurry", "Cecilia", "Vesta", "Terry", "Patrick"
        };
        private static readonly string[] _lastNames = new string[] {
            "Doe", "Smith", "Johns", "Gates", "Carmack", "Zontak", "Clinton", "Adams", "First", "Lopez", "Due", "White", "Black"
        };
        private static readonly string[] _colors = new string[] {
            "Black", "White", "Red", "Blue", "Green", "Yellow", "Purple", "Grey", "Magenta", "Cian"
        };
        private static readonly string[] _stuffs = new string[] {
            "Game", "Ball", "Home", "Board", "Car", "Plane", "Hotel", "Wine", "Pants", "Boots", "Table", "Chair"
        };
        private static readonly string[] _adjectives = new string[] {
            "Large", "Small", "High", "Low", "Certain", "Fuzzy", "Modern", "Faster", "Slower"
        };
        private static readonly string[] _verbs = new string[] {
            "Run", "Stay", "Breeze", "Fly", "Lay", "Write", "Draw", "Scream"
        };
        //private static readonly string[] _streetTypes = new string[] {
        //    "Lane", "Court", "Circle", "Drive", "Way", "Loop", "Blvd", "Street"
        //};
        //private static readonly string[] _streetPrefix = new string[] {
        //    "North", "South", "East", "West", "Old", "New", "N.", "S.", "E.", "W."
        //};
        //private static readonly string[] _streetNames = new string[] {
        //    "1st", "2nd", "3rd", "4th", "53rd", "6th", "8th", "Acacia", "Academy", "Adams", "Addison", "Airport", "Albany", "Alderwood", "Alton", "Amerige", "Amherst", "Anderson",
        //    "Ann", "Annadale", "Applegate", "Arcadia", "Arch", "Argyle", "Arlington", "Armstrong", "Arnold", "Arrowhead", "Aspen", "Augusta", "Baker", "Bald Hill", "Bank", "Bay Meadows",
        //    "Bay", "Bayberry", "Bayport", "Beach", "Beaver Ridge", "Bedford", "Beech", "Beechwood", "Belmont", "Berkshire", "Big Rock Cove", "Birch Hill", "Birchpond", "Birchwood",
        //    "Bishop", "Blackburn", "Blue Spring", "Bohemia", "Border", "Boston", "Bow Ridge", "Bowman", "Bradford", "Brandywine", "Brewery", "Briarwood", "Brickell", "Brickyard",
        //    "Bridge", "Bridgeton", "Bridle", "Broad", "Brookside", "Brown", "Buckingham", "Buttonwood", "Cambridge", "Campfire", "Canal", "Canterbury", "Cardinal", "Carpenter",
        //    "Carriage", "Carson", "Catherine", "Cedar Swamp", "Cedar", "Cedarwood", "Cemetery", "Center", "Central", "Chapel", "Charles", "Cherry Hill", "Chestnut", "Church", "Circle",
        //    "Clark", "Clay", "Cleveland", "Clinton", "Cobblestone", "Coffee", "College", "Colonial", "Columbia", "Cooper", "Corona", "Cottage", "Country Club", "Country", "County", "Court",
        //    "Courtland", "Creek", "Creekside", "Crescent", "Cross", "Cypress", "Deerfield", "Del Monte", "Delaware", "Depot", "Devon", "Devonshire", "Division", "Dogwood", "Dunbar",
        //    "Durham", "Eagle", "East", "Edgefield", "Edgemont", "Edgewater", "Edgewood", "El Dorado", "Elizabeth", "Elm", "Essex", "Euclid", "Evergreen", "Fairfield", "Fairground", "Fairview",
        //    "Fairway", "Fawn", "Fifth", "Fordham", "Forest", "Foster", "Foxrun", "Franklin", "Fremont", "Front", "Fulton", "Galvin", "Garden", "Gartner", "Gates", "George", "Glen Creek",
        //    "Glen Eagles", "Glen Ridge", "Glendale", "Glenlake", "Glenridge", "Glenwood", "Golden Star", "Goldfield", "Golf", "Gonzales", "Grand", "Grandrose", "Grant", "Green Hill",
        //    "Green Lake", "Green", "Greenrose", "Greenview", "Gregory", "Griffin", "Grove", "Halifax", "Hamilton", "Hanover", "Harrison", "Hartford", "Harvard", "Harvey", "Hawthorne",
        //    "Heather", "Henry Smith", "Heritage", "High Noon", "High Point", "High", "Highland", "Hill Field", "Hillcrest", "Hilldale", "Hillside", "Hilltop", "Holly", "Homestead",
        //    "Homewood", "Honey Creek", "Howard", "Indian Spring", "Indian Summer", "Iroquois", "Jackson", "James", "Jefferson", "Jennings", "Jockey Hollow", "John", "Johnson", "Jones",
        //    "Joy Ridge", "King", "Kingston", "Kirkland", "La Sierra", "Lafayette", "Lake Forest", "Lake", "Lakeshore", "Lakeview", "Lancaster", "Lane", "Laurel", "Leatherwood", "Lees Creek",
        //    "Leeton Ridge", "Lexington", "Liberty", "Lilac", "Lincoln", "Linda", "Littleton", "Livingston", "Locust", "Longbranch", "Lookout", "Lower River", "Lyme", "Madison", "Maiden",
        //    "Main", "Mammoth", "Manchester", "Manhattan", "Manor Station", "Maple", "Marconi", "Market", "Marsh", "Marshall", "Marvon", "Mayfair", "Mayfield", "Mayflower", "Meadow",
        //    "Meadowbrook", "Mechanic", "Middle River", "Miles", "Mill Pond", "Miller", "Monroe", "Morris", "Mountainview", "Mulberry", "Myrtle", "Newbridge", "Newcastle", "Newport",
        //    "Nichols", "Nicolls", "North", "Nut Swamp", "Oak Meadow", "Oak Valley", "Oak", "Oakland", "Oakwood", "Ocean", "Ohio", "Oklahoma", "Olive", "Orange", "Orchard", "Overlook",
        //    "Pacific", "Paris Hill", "Park", "Parker", "Pawnee", "Peachtree", "Pearl", "Peg Shop", "Pendergast", "Peninsula", "Penn", "Pennington", "Pennsylvania", "Pheasant", "Philmont",
        //    "Pierce", "Pin Oak", "Pine", "Pineknoll", "Piper", "Plumb Branch", "Poor House", "Prairie", "Primrose", "Prince", "Princess", "Princeton", "Proctor", "Prospect", "Pulaski",
        //    "Pumpkin Hill", "Purple Finch", "Queen", "Race", "Ramblewood", "Redwood", "Ridge", "Ridgewood", "River", "Riverside", "Riverview", "Roberts", "Rock Creek", "Rock Maple",
        //    "Rockaway", "Rockcrest", "Rockland", "Rockledge", "Rockville", "Rockwell", "Rocky River", "Roosevelt", "Rose", "Rosewood", "Ryan", "Saddle", "Sage", "San Carlos", "San Juan",
        //    "San Pablo", "Santa Clara", "Saxon", "School", "Schoolhouse", "Second", "Shadow Brook", "Shady", "Sheffield", "Sherman", "Sherwood", "Shipley", "Shub Farm", "Sierra",
        //    "Silver Spear", "Sleepy Hollow", "Smith Store", "Smoky Hollow", "Snake Hill", "Southampton", "Spring", "Spruce", "Squaw Creek", "St Louis", "St Margarets", "St Paul", "State",
        //    "Stillwater", "Strawberry", "Studebaker", "Sugar", "Sulphur Springs", "Summerhouse", "Summit", "Sunbeam", "Sunnyslope", "Sunset", "Surrey", "Sutor", "Swanson", "Sycamore",
        //    "Tailwater", "Talbot", "Tallwood", "Tanglewood", "Tarkiln Hill", "Taylor", "Thatcher", "Third", "Thomas", "Thompson", "Thorne", "Tower", "Trenton", "Trusel", "Tunnel",
        //    "University", "Vale", "Valley Farms", "Valley View", "Valley", "Van Dyke", "Vermont", "Vernon", "Victoria", "Vine", "Virginia", "Wagon", "Wall", "Walnutwood", "Warren",
        //    "Washington", "Water", "Wayne", "Westminster", "Westport", "White", "Whitemarsh", "Wild Rose", "William", "Williams", "Wilson", "Winchester", "Windfall", "Winding Way",
        //    "Winding", "Windsor", "Wintergreen", "Wood", "Woodland", "Woodside", "Woodsman", "Wrangler", "York",
        //};

        private static readonly string[] _allWords = _firstNames.Concat(_lastNames).Concat(_colors).Concat(_stuffs).Concat(_adjectives)
            .Concat(_verbs).ToArray();

        public static string Color()
        {
            return RandomString.Pick(_colors);
        }

        public static string Stuff()
        {
            return RandomString.Pick(_stuffs);
        }

        public static string Adjective()
        {
            return RandomString.Pick(_adjectives);
        }

        public static string Verb()
        {
            return RandomString.Pick(_verbs);
        }

        public static string Phrase(int size)
        {
            return Phrase(size, size);
        }

        public static string Phrase(int minSize, int maxSize)
        {
            maxSize = Math.Max(minSize, maxSize);
            int size = RandomInteger.NextInteger(minSize, maxSize);
            if (size <= 0) return "";

            StringBuilder result = new StringBuilder();
            result.Append(RandomString.Pick(_allWords));
            while (result.Length < size)
            {
                result.Append(" ").Append(RandomString.Pick(_allWords).ToLower());
            }

            return result.ToString();
        }

        public static string Name()
        {
            StringBuilder result = new StringBuilder();

            if (RandomBoolean.Chance(3, 5))
                result.Append(RandomString.Pick(_namePrefixes)).Append(" ");

            result.Append(RandomString.Pick(_firstNames))
                .Append(" ")
                .Append(RandomString.Pick(_lastNames));

            if (RandomBoolean.Chance(5, 10))
                result.Append(" ").Append(RandomString.Pick(_nameSuffixes));

            return result.ToString();
        }

        public static string Word()
        {
            return RandomString.Pick(_allWords);
        }

        public static string Words(int min, int max)
        {
            StringBuilder result = new StringBuilder();

            int count = RandomInteger.NextInteger(min, max);
            for (int i = 0; i < count; i++)
                result.Append(RandomString.Pick(_allWords));

            return result.ToString();
        }

        public static string Phone()
        {
            StringBuilder result = new StringBuilder();

            result.Append("(")
                .Append(RandomInteger.NextInteger(111, 999))
                .Append(") ")
                .Append(RandomInteger.NextInteger(111, 999))
                .Append("-")
                .Append(RandomInteger.NextInteger(0, 9999));

            return result.ToString();
        }

        public static string Email()
        {
            return Words(2, 6) + "@" + Words(1, 3) + ".com";
        }

        public static string Text(int size)
        {
            return Text(size, size);
        }

        public static string Text(int minSize, int maxSize)
        {
            maxSize = Math.Max(minSize, maxSize);
            int size = RandomInteger.NextInteger(minSize, maxSize);

            StringBuilder result = new StringBuilder();
            result.Append(RandomString.Pick(_allWords));

            while (result.Length < size)
            {
                String next = RandomString.Pick(_allWords);
                if (RandomBoolean.Chance(4, 6))
                    next = " " + next.ToLower();
                else if (RandomBoolean.Chance(2, 5))
                    next = RandomString.Pick(":,-") + next.ToLower();
                else if (RandomBoolean.Chance(3, 5))
                    next = RandomString.Pick(":,-") + " " + next.ToLower();
                else
                    next = RandomString.Pick(".!?") + " " + next;

                result.Append(next);
            }

            return result.ToString();
        }
    }
}
