﻿
using Microsoft.AspNetCore.Identity;
using partner_aluro.DAL;
using partner_aluro.Models;
using partner_aluro.Services.Interfaces;
using System.Reflection;
using System.Xml.Linq;

namespace partner_aluro.Services
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;
        public OrderService(ApplicationDbContext context)
        {
            _context = context;
        }
        public OrderItem GetItem(int id)
        {
            var orderItem = _context.OrderItems.Find(id);

            return orderItem;
        }

        public Order GetOrder(int id)
        {
            var order = _context.Orders.Find(id);
            var user = _context.Users.Find(order.UserID);
            order.User = user;

            return order;
        }

        public Adress1 GetUserAdress1(string UserID)
        {
            List<Adress1> ListaAdresow = _context.Adress1.ToList();
            Adress1 adress1 = new Adress1();

            foreach (var adres in ListaAdresow)
            {
                if (adres.UserID == UserID)
                {
                    adress1 = adres;
                    break;
                }
            }


            return adress1;
        }

        public Adress2 GetUserAdress2(string UserID)
        {
            throw new NotImplementedException();
        }

        public List<OrderItem> List()
        {
            var OrderItems = _context.OrderItems.ToList();
            return OrderItems;
        }

        public List<OrderItem> List(int id)
        {
            var OrderItem = _context.OrderItems.ToList();

            List<OrderItem> OrderId = new List<OrderItem>();

            foreach(var item in OrderItem)
            {
                if(item.OrderId == id)
                {
                    OrderId.Add(item);
                }
            }

            var ListaProduktow = _context.Products.ToList();

            foreach (var itemOrd in OrderItem)
            {
                foreach (var produkt in ListaProduktow)
                {
                    if (itemOrd.ProductId == produkt.ProductId)
                    {
                        itemOrd.Product = produkt;
                    }
                }
            }

            return OrderId;
        }
    }
}