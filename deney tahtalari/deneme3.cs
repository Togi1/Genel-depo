string ehliyet;
string araba;
Console.Write("ehliyetin var mı (var yada yok de)");
ehliyet = Console.ReadLine();
Console.Write("araban var mı (var yada yok de )");
araba = Console.ReadLine();

if (ehliyet == "var" && araba == "var")
{
    Console.WriteLine("sür babaaa");
}
else if (ehliyet == "yok" && araba == "yok")
{
    Console.WriteLine("mümkünse yolda yürüme");
}
else if (ehliyet == "var" || araba == "yok")
{
    Console.WriteLine("trafiğe çıkma pls");
}
else if (ehliyet == "yok" || araba == "var")
{
    Console.WriteLine("trafiğe çıkma pls");
}
