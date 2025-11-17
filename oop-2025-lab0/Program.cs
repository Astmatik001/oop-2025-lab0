using System;
using System.Collections.Generic;
using System.Linq;

public class VendingMachine
{
    private Dictionary<Product, int> products = new Dictionary<Product, int>();
    private Dictionary<int, int> insertedCoins = new Dictionary<int, int>();
    private int balance = 0;
    private int collectedFunds = 0;
    public VendingMachine()
    {
        products = new Dictionary<Product, int>
        {
            { new Product("Шоколадка", 50), 5 },
            { new Product("Чипсы", 65), 3 },
            { new Product("Газировка", 80), 2 }
        };

        insertedCoins = new Dictionary<int, int>();
        balance = 0;
        collectedFunds = 0;
    }

    public void ShowProducts()
    {
        Console.WriteLine("Доступные товары:");
        int i = 1;
        foreach (KeyValuePair<Product, int> item in products)
        {
            Console.WriteLine($"{i}. {item.Key.Name} - {item.Key.Price} руб. (в наличии: {item.Value})");
            i++;
        }
    }
    public void InsertCoin(int coin)
    {
        int[] validCoins = { 1, 5, 10, 25, 50, 100 };
        if (Array.IndexOf(validCoins, coin) == -1)
        {
            Console.WriteLine("Номинал монеты не принимается.");
            return;
        }
        if (!insertedCoins.ContainsKey(coin))
            insertedCoins[coin] = 0;
        insertedCoins[coin]++;
        balance += coin;
        Console.WriteLine($"Внесено {coin} руб. Текущий баланс: {balance} руб.");
    }
    public void ReturnChange()
    {
        Console.WriteLine($"Возврат сдачи {balance} руб.");
        insertedCoins.Clear();
        balance = 0;
    }
    public void CancelTransaction()
    {
        Console.WriteLine("Отмена операции. Возврат внесённых монет:");
        foreach (var coin in insertedCoins)
        {
            Console.WriteLine($"{coin.Key} руб. - {coin.Value} шт.");
        }
        insertedCoins.Clear();
        balance = 0;
    }
    public void ChooseProduct(int index)
    {
        if (index < 1 || index > products.Count)
        {
            Console.WriteLine("Неверный выбор товара.");
            return;
        }
        var product = products.Keys.ElementAt(index - 1);
        int quantity = products[product];

        if (quantity == 0)
        {
            Console.WriteLine("Товар закончился.");
            return;
        }
        if (balance < product.Price)
        {
            Console.WriteLine($"Недостаточно средств. Цена: {product.Price}, Баланс: {balance}");
            return;
        }

        products[product] = quantity - 1;
        balance -= product.Price;
        collectedFunds += product.Price;

        Console.WriteLine($"Вы получили: {product.Name}. Сдача: {balance} руб.");

        ReturnChange();
    }

    public void AdminMode()
    {
        Console.WriteLine("Вход в режим администратора:");
        Console.Write("Введите пароль: ");
        string password = Console.ReadLine();
        if (password != "admin123")
        {
            Console.WriteLine("Неверный пароль.");
            return;
        }
        while (true)
        {
            Console.WriteLine("1. Пополнить товар\n2. Снять средства\n3. Показать товары\n4. Выйти из режима администратора");
            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    Console.WriteLine("Выберите товар для пополнения:");
                    ShowProducts();
                    if (int.TryParse(Console.ReadLine(), out int idx) && idx >= 1 && idx <= products.Count)
                    {
                        var productToUpdate = products.Keys.ElementAt(idx - 1);
                        Console.Write("Введите количество для добавления: ");
                        if (int.TryParse(Console.ReadLine(), out int qty) && qty > 0)
                        {
                            products[productToUpdate] += qty;
                            Console.WriteLine("Товар успешно пополнен.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Неверный номер товара.");
                    }
                    break;
                case "2":
                    Console.WriteLine($"Снято средств: {collectedFunds} руб.");
                    collectedFunds = 0;
                    break;
                case "3":
                    ShowProducts();
                    break;
                case "4":
                    return;
                default:
                    Console.WriteLine("Неверный выбор.");
                    break;
            }
        }
    }
}
public class Product
{
    public string Name { get; private set; }
    public int Price { get; private set; }
    public Product(string name, int price)
    {
        Name = name;
        Price = price;
    }
}
public class UserInterface
{
    
}

public class Program
{
    static void Main(string[] args)
    {
        VendingMachine vm = new VendingMachine();

        while (true)
        {
            Console.WriteLine("\n1. Показать товары\n2. Вставить монету\n3. Выбрать товар\n4. Отмена операции\n5. Режим администратора\n0. Выход");
            string input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    vm.ShowProducts();
                    break;
                case "2":
                    Console.Write("Введите номинал монеты (1,5,10,25,50,100): ");
                    if (int.TryParse(Console.ReadLine(), out int coin))
                        vm.InsertCoin(coin);
                    else
                        Console.WriteLine("Ошибка ввода.");
                    break;
                case "3":
                    vm.ShowProducts();
                    Console.Write("Введите номер товара: ");
                    if (int.TryParse(Console.ReadLine(), out int prodNum))
                        vm.ChooseProduct(prodNum);
                    else
                        Console.WriteLine("Ошибка ввода.");
                    break;
                case "4":
                    vm.CancelTransaction();
                    break;
                case "5":
                    vm.AdminMode();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Неверная команда.");
                    break;
            }
        }
    }
}