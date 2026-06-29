// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");
Console.WriteLine($"17 mod 4 = {ModOperator(17, 4)}");

static int ModOperator(int a, int b)
{
    return a % b;
}

Console.Write("Nhập số thứ nhất: ");
double num1 = Convert.ToDouble(Console.ReadLine());

Console.Write("Nhập số thứ hai: ");
double num2 = Convert.ToDouble(Console.ReadLine());

double result = num1 * num2;

Console.WriteLine($"Kết quả của {num1} * {num2} = {result}");