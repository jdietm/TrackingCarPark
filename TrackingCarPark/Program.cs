class Program
{
    static void Main(string[] args)
    {
        /* declare a new VehicleTracker class, with a capacity for 10 cars, on 123 Fake St.

        Construct a Dictionary of 10 key-value pairs {number, parked car}, where parked car is null and values are 1 - 10 */

        VehicleTracker vt = new VehicleTracker(10, "123 Fake st");

        // declare a new Vehicle with the licence place “A01 T22”, and with a parking pass (bool true)

        //- Vehicle customerOne = new Vehicle("A01 T22", true);

        /* Add a vehicle to vt’s VehicleList property. It replaces the first “unoccupied” value in VehicleList, so we expect vt.VehicleList = {{1, customerOne}, {2, null}, {3, null}, etc} */

        //-vt.AddVehicle(customerOne);

        // Change the value of slot 1 in VehicleList to null 

        //-vt.RemoveVehicle("A0T T22");
        Console.WriteLine(vt.Capacity);
        Console.WriteLine(vt.SlotsAvailable);
        Console.WriteLine(vt.VehicleList.Count());

    }
}

public class Vehicle
{
    public string Licence { get; set; }
    public bool Pass { get; set; }
    public Vehicle(string licence, bool pass)
    {
        this.Licence = licence;
        this.Pass = pass;
    }
}

public class VehicleTracker
{
    //PROPERTIES
    public string Address { get; set; }
    public int Capacity { get; set; }
    public int SlotsAvailable { get; set; }
    public Dictionary<int, Vehicle> VehicleList { get; set; }

    public VehicleTracker(int capacity, string address)
    {
        this.Capacity = capacity;
        this.Address = address;
        this.VehicleList = new Dictionary<int, Vehicle>();

        this.GenerateSlots();
    }

    // STATIC PROPERTIES
    public static string BadSearchMessage = "Error: Search did not yield any result.";
    public static string BadSlotNumberMessage = "Error: No slot with number ";
    public static string SlotsFullMessage = "Error: no slots available.";

    // METHODS
    public void GenerateSlots()
    {
        //for (int i = 0; i <= this.Capacity; i++) 
        for (int i = 0; i < this.Capacity; i++)    // This code fix the capacity issue UnitTest_1()
        {
            this.VehicleList.Add(i, null);
        }
        this.SlotsAvailable = this.VehicleList.Count;// This code fix the capacity issue UnitTest_5()
    }

    public void AddVehicle(Vehicle vehicle)
    {
        foreach (KeyValuePair<int, Vehicle> slot in this.VehicleList)
        {
            if (slot.Value == null)
            {
                this.VehicleList[slot.Key] = vehicle;
                //this.SlotsAvailable++;
                this.SlotsAvailable--;  // This fixes the issue in UnitTest_5();
                return;
            }
        }
        throw new IndexOutOfRangeException(SlotsFullMessage);
    }

    public void RemoveVehicle(string licence)
    {
        try
        {
            int slot = this.VehicleList.First(v => v.Value.Licence == licence).Key;
            this.SlotsAvailable++;  // This fixes the issue in UnitTest_5();
            this.VehicleList[slot] = null;
        }
        catch
        {
            throw new NullReferenceException(BadSearchMessage);
        }
    }

    public bool RemoveVehicle(int slotNumber)
    {
        // This code fix the Exception issue UnitTest_4() 

        if (slotNumber > this.Capacity || slotNumber < 0)
        {
            throw new NullReferenceException(BadSearchMessage); 
            return false;
        }
        try
        {
            if (this.VehicleList[slotNumber] != null)
            {
                this.VehicleList[slotNumber] = null;
                 this.SlotsAvailable++; 
            }
            //} else
            //{
            //    throw new NullReferenceException(BadSearchMessage);  // slot number not found
            //    return false;
            //}
            return true;
        }
        catch
        {
            throw new NullReferenceException(BadSearchMessage);
        }
    }

    public List<Vehicle> ParkedPassholders()
    {
        List<Vehicle> passHolders = new List<Vehicle>();

        // The following code fixes UnitTest_6()
        //passHolders.Add(this.VehicleList.FirstOrDefault(v => v.Value.Pass).Value); 
        foreach (KeyValuePair<int, Vehicle> slot in this.VehicleList)
        {
            if (slot.Value.Pass == true)
            {
                passHolders.Add(slot.Value);
            }
        }
        return passHolders;
    }

    public int PassholderPercentage()
    {
        int passHolders = ParkedPassholders().Count();
        int percentage = (passHolders / this.Capacity) * 100;
        return percentage;
    }
}

