using Microsoft.VisualStudio.TestTools.UnitTesting;
using SurfsUpLibrary.Models;
using SurfsUpLibrary.Models.Repositories;

namespace SurfsUpUnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async Task TestEquipmentRepo()
        {
            Equipment equipment = new()
            {
                Name = "Spade"
            };

            List<Equipment> equipmentList = await EquipmentRepo.Retrieve();
            int countBefore = equipmentList.Count;

            Equipment added = await EquipmentRepo.Create(equipment);

            equipmentList = await EquipmentRepo.Retrieve();
            int countAfter = equipmentList.Count;

            Assert.AreEqual(countBefore + 1, countAfter);

            await EquipmentRepo.Delete(added);

            equipmentList = await EquipmentRepo.Retrieve();
            int countDelete = equipmentList.Count;

            Assert.AreEqual(countBefore, countDelete);
        }
    }
}