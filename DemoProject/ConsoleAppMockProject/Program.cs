// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

Console.Write("Nhập số thứ nhất: ");
double num1 = Convert.ToDouble(Console.ReadLine());

Console.Write("Nhập số thứ hai: ");
double num2 = Convert.ToDouble(Console.ReadLine());

double result = num1 - num2;

Console.WriteLine($"Kết quả của {num1} - {num2} = {result}");
