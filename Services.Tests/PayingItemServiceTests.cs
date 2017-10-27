using System.Collections.Generic;
using System.Linq;
using BussinessLogic.Services;
using DomainModels.Model;
using DomainModels.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WebUI.Models;

namespace Services.Tests
{
    [TestClass]
    public class PayingItemServiceTests
    {
        private readonly Mock<IRepository<PayingItem>> _repository;
        private readonly PayingItemService _service;

        private readonly List<PayingItem> _listOfItems = new List<PayingItem>()
        {
            new PayingItem() {UserId = "1", Category = new Category() {TypeOfFlowID = 1}},
            new PayingItem() {UserId = "1", Category = new Category() {TypeOfFlowID = 1}},
            new PayingItem() {UserId = "2", Category = new Category() {TypeOfFlowID = 2}}
        };

        public PayingItemServiceTests()
        {
            _repository = new Mock<IRepository<PayingItem>>();
            _service = new PayingItemService(_repository.Object);
        }

       
    }
}
