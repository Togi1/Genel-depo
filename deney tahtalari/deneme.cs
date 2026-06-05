using System.Data.Common;

int yas;

Console.Write("yasınız");
yas = Convert.ToInt32(Console.ReadLine());
if (yas >=18)
{
    Console.WriteLine("hosgeldin");
}
else if (yas <= 0)
{
    Console.WriteLine("yasin bozuk");
}
else if (yas < 18)
{
    Console.WriteLine("sen burdan gecemen");
}