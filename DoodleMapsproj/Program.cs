using Kse.Algorithms.Samples;

var generator = new MapGenerator(new MapGeneratorOptions()
{
    Height = 35,
    Width = 90,
    Noise = .1f,
    AddTraffic = true
});

string[,] map = generator.Generate();
var printer = new MapPrinter();
var pathFinder = new PathFinder();
var shortestPath =  pathFinder.GetShortestPath(map, new Point(0, 34),new Point(61, 0));
printer.Print(map, shortestPath);