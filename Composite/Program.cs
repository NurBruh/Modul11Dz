using System;
using System.Collections.Generic;

public interface IHotelRepository
{
    void AddHotel(string name, string location);
    List<string> GetHotels(string location);
}

public interface IBookingManager
{
    bool ReserveRoom(string hotelName, string user, DateTime start, DateTime end);
}

public interface IPaymentHandler
{
    bool MakePayment(string user, decimal amount);
}

public interface INotificationManager
{
    void Notify(string user, string message);
}

public interface IUserService
{
    void AddUser(string name, string contact);
    bool VerifyUser(string name);
}

public class HotelRepository : IHotelRepository
{
    private readonly List<string> hotels = new();

    public void AddHotel(string name, string location)
    {
        hotels.Add($"{name} ({location})");
    }

    public List<string> GetHotels(string location)
    {
        return hotels.FindAll(h => h.Contains(location));
    }
}

public class BookingManager : IBookingManager
{
    public bool ReserveRoom(string hotelName, string user, DateTime start, DateTime end)
    {
        Console.WriteLine($"Бронирование подтверждено: {hotelName} для {user} с {start.ToShortDateString()} по {end.ToShortDateString()}.");
        return true;
    }
}

public class PaymentHandler : IPaymentHandler
{
    public bool MakePayment(string user, decimal amount)
    {
        Console.WriteLine($"Платеж пользователя {user} на сумму {amount} успешно выполнен.");
        return true;
    }
}

public class NotificationManager : INotificationManager
{
    public void Notify(string user, string message)
    {
        Console.WriteLine($"Уведомление для {user}: {message}");
    }
}

public class UserService : IUserService
{
    private readonly HashSet<string> users = new();

    public void AddUser(string name, string contact)
    {
        users.Add(name);
        Console.WriteLine($"Пользователь {name} зарегистрирован с контактной информацией: {contact}");
    }

    public bool VerifyUser(string name)
    {
        return users.Contains(name);
    }
}

public class HotelSystem
{
    private readonly IHotelRepository hotelRepo;
    private readonly IBookingManager bookingManager;
    private readonly IPaymentHandler paymentHandler;
    private readonly INotificationManager notificationManager;
    private readonly IUserService userService;

    public HotelSystem(
        IHotelRepository hotelRepo,
        IBookingManager bookingManager,
        IPaymentHandler paymentHandler,
        INotificationManager notificationManager,
        IUserService userService)
    {
        this.hotelRepo = hotelRepo;
        this.bookingManager = bookingManager;
        this.paymentHandler = paymentHandler;
        this.notificationManager = notificationManager;
        this.userService = userService;
    }

    public void Start()
    {
        userService.AddUser("Nurik", "alinur@gmail.com");

        if (userService.VerifyUser("Nurgeldi Sh."))
        {
            hotelRepo.AddHotel("Отель Казахстан", "Алматы");
            var hotels = hotelRepo.GetHotels("Алматы");

            Console.WriteLine("Доступные отели: " + string.Join(", ", hotels));

            if (bookingManager.ReserveRoom("Отель Казахстан", "Nurgeldi Sh.", DateTime.Today, DateTime.Today.AddDays(2)))
            {
                if (paymentHandler.MakePayment("Nurgeldi Sh.", 5000))
                {
                    notificationManager.Notify("Nurgeldi Sh.", "Ваше бронирование подтверждено!");
                }
            }
        }
    }
}

class Program
{
    static void Main()
    {
        var system = new HotelSystem(
            new HotelRepository(),
            new BookingManager(),
            new PaymentHandler(),
            new NotificationManager(),
            new UserService());

        system.Start();
    }
}
