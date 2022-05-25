using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TrackingCarPark_UnitTest
{
    [TestClass]
    public class TrackingCarPark
    {
        
        VehicleTracker vt = new VehicleTracker(3, "123 Fake st");

        [TestMethod]
        public void UnitTest_1()
        {
            bool allSlotsNull = true;
            foreach (KeyValuePair<int, Vehicle> slot in vt.VehicleList)
            {
                if (slot.Value != null)
                {
                  allSlotsNull = false; 
                }
            }
            int numberOfSlots = vt.VehicleList.Count;   


            Assert.AreEqual(numberOfSlots, vt.Capacity);
            Assert.AreEqual(allSlotsNull, true);

        }
        [TestMethod]
        public void UnitTest_2()
        {
            int firstSlotPosition = 0;
            int vehicleSlotPosition = 0;
            Vehicle customerOne = new Vehicle("A00 S01", true);
            Vehicle customerTwo = new Vehicle("A00 S02", true);
            Vehicle customerThree = new Vehicle("A00 S03", true);
            Vehicle customerFour = new Vehicle("A00 S04", true);
           
            vt.AddVehicle(customerOne);
            vt.AddVehicle(customerTwo);

            foreach (KeyValuePair<int, Vehicle> slot in vt.VehicleList)
            {
                if (slot.Value == null)
                {
                    break;
                }
                firstSlotPosition++;
            }
           
            vt.AddVehicle(customerThree);

            foreach (KeyValuePair<int, Vehicle> slot in vt.VehicleList)
            {
                if (slot.Value == null)
                {
                    break;
                }
                vehicleSlotPosition++;
            }
            
            Assert.AreEqual(firstSlotPosition+1, vehicleSlotPosition);  // first slot in VehicleList that is not full. 
           
            Assert.ThrowsException<IndexOutOfRangeException>(() =>       // If there are no open slots
               vt.AddVehicle(customerFour));

        }

        [TestMethod]
        public void UnitTest_3()
        {
            int initialVehiclesParked = 0;

            foreach (KeyValuePair<int, Vehicle> slot in vt.VehicleList)
            {
                if (slot.Value == null)
                {
                    break;
                }
                initialVehiclesParked++;
            }

            Vehicle customerOne = new Vehicle("A00 S01", true);
            vt.AddVehicle(customerOne);
            int currentVehiclesParked = initialVehiclesParked + 1;
            //RemoveVehicle should accept either a licence 
            if (initialVehiclesParked == currentVehiclesParked - 1)
            {
                vt.RemoveVehicle("A00 S01");
                currentVehiclesParked--;
            }
            Assert.AreEqual(initialVehiclesParked, currentVehiclesParked);  // Remove by licence

            vt.AddVehicle(customerOne);
            currentVehiclesParked = initialVehiclesParked + 1;

            //RemoveVehicle should accept slot number
            if (initialVehiclesParked == currentVehiclesParked - 1)
            {
                vt.RemoveVehicle(currentVehiclesParked);
                currentVehiclesParked--;
            }
            Assert.AreEqual(initialVehiclesParked, currentVehiclesParked);  // Remove by slot
        }

        [TestMethod]
        public void UnitTest_4()
        {
            Vehicle customerOne = new Vehicle("A00 S01", true);
            vt.AddVehicle(customerOne);


            Assert.ThrowsException<NullReferenceException>(() =>       // licence passed to RemoveVehicle() is not found
               vt.RemoveVehicle("BAD LIC"));
            Assert.ThrowsException<NullReferenceException>(() =>       // slot passed to RemoveVehicle() is Grater than Capacity
               vt.RemoveVehicle(vt.VehicleList.Count+1));
            Assert.ThrowsException<NullReferenceException>(() =>       // slot passed to RemoveVehicle() is not found
               vt.RemoveVehicle(-1));
            
        }

        [TestMethod]
        public void UnitTest_5()
        {
            int slotsAvailable = vt.VehicleList.Count;

            Vehicle customerOne = new Vehicle("A00 S01", true);
            vt.AddVehicle(customerOne);
            
            
            slotsAvailable = slotsAvailable - 1;
            Assert.AreEqual(slotsAvailable, vt.SlotsAvailable);  // track the proper number of slots available after adding 

            vt.RemoveVehicle("A00 S01");
            slotsAvailable = slotsAvailable + 1;

            Assert.AreEqual(slotsAvailable, vt.SlotsAvailable); // track the proper number of slots available after removing 

        }

        [TestMethod]
        public void UnitTest_6()
        {
            Vehicle customerOne = new Vehicle("A00 S01", true);
            Vehicle customerTwo = new Vehicle("A00 S02", false);
            Vehicle customerThree = new Vehicle("A00 S03", true);
          
            vt.AddVehicle(customerOne);
            vt.AddVehicle(customerTwo);
            vt.AddVehicle(customerThree);
           

            List<Vehicle> passHoldersTest = vt.ParkedPassholders();

            Assert.AreEqual(passHoldersTest.Count, 2);
        }

        [TestMethod]
        public void UnitTest_7()
        {
            Vehicle customerOne = new Vehicle("A00 S01", true);
            Vehicle customerTwo = new Vehicle("A00 S02", true);
            Vehicle customerThree = new Vehicle("A00 S03", true);

            vt.AddVehicle(customerOne);
            vt.AddVehicle(customerTwo);
            vt.AddVehicle(customerThree);

            int percentageTest = vt.PassholderPercentage();



            Assert.AreEqual(percentageTest, 100);
        }


    }
}


/*     Unit Test List
 * ------------------






   UnitTest_1()
1) When initialized, a VehicleTracker object should have empty slots [{SlotNumber, Vehicle}] from 1 - Capacity in VehicleTracker.VehicleList 
   (ie. { {1, null}, {2, null}, {3,null}, //etc}
 
   UnitTest_2()
2) If the AddVehicle method is called, it should add the vehicle to the first slot in VehicleList that is not full. 
   If there are no open slots, it should throw a generic exception with the VehicleTracker.AllSlotsFull message.

   UnitTest_3() 
3) RemoveVehicle should accept either a licence or slot number, and set that slot’s vehicle to “null”.

   UnitTest_4()
4) RemoveVehicle should throw an exception if the licence passed to RemoveVehicle() is not found, if the slot number is invalid (greater than capacity or negative), 
   or the slot is not filled. 

   UnitTest_5()
5) VehicleTracker should track the proper number of slots available at all times with VehicleTracker.SlotsAvailable.

6) The VehicleTracker.ParkedPassholders() method should return a list of all parked vehicles that have a pass.

7) VehicleTracker.PassholderPercentage() method should return the percentage of vehicles that are parked which have passes. 
   Note that this method uses the ParkedPassholders() method to get a count of passholders.


 */