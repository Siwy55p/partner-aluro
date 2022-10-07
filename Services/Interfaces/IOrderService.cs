using partner_aluro.Models;

namespace partner_aluro.Services.Interfaces
{
    public interface IOrderService
    {
        List<OrderItem> List(int id);
        List<OrderItem> List();
        OrderItem GetItem(int id);

        Order GetOrder(int id);

        Adress1rozliczeniowy GetUserAdress1(string UserID);
        Adress2dostawy GetUserAdress2(string UserID);

    }
}
