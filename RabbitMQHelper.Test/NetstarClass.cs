using System;

namespace RabbitMQHelper.Test
{
    public class NetstarTrip
    {
        public Start Start { get; set; }
        public End End { get; set; }
        public float AverageSpeed { get; set; }
        public float MaxSpeed { get; set; }
        public float Distance { get; set; }
        public bool IsComplete { get; set; }
        public int Error { get; set; }
        public Location[] Locations { get; set; }
        public string Id { get; set; }
        public long Imei { get; set; }
        public int VbuNo { get; set; }
        public int UnitType { get; set; }
        public DateTime ServerDateTime { get; set; }
    }

    public class Start
    {
        public DateTime DateTime { get; set; }
        public float Longitude { get; set; }
        public float Latitude { get; set; }
        public float Speed { get; set; }
        public float Heading { get; set; }
        public float Odometer { get; set; }
        public bool IsIgnitionOn { get; set; }
        public int EventId { get; set; }
        public string DriverId { get; set; }
        public int AccidentId { get; set; }
        public bool IsLastKnowLocation { get; set; }
        public float Altitude { get; set; }
        public float Battery { get; set; }
        public float EngineHours { get; set; }
        public Address Address { get; set; }
        public string Id { get; set; }
        public int Imei { get; set; }
        public int VbuNo { get; set; }
        public int UnitType { get; set; }
        public DateTime ServerDateTime { get; set; }
    }

    public class Address
    {
        public string Country { get; set; }
        public string Province { get; set; }
        public string Municipality { get; set; }
        public string Town { get; set; }
        public string Suburb { get; set; }
        public string Street { get; set; }
        public int SpeedLimit { get; set; }
        public bool SpeedLimitVerified { get; set; }
        public int RoadClassification { get; set; }
        public int RoadCondition { get; set; }
        public bool RoughRoad { get; set; }
    }

    public class End
    {
        public DateTime DateTime { get; set; }
        public float Longitude { get; set; }
        public float Latitude { get; set; }
        public float Speed { get; set; }
        public float Heading { get; set; }
        public float Odometer { get; set; }
        public bool IsIgnitionOn { get; set; }
        public int EventId { get; set; }
        public string DriverId { get; set; }
        public int AccidentId { get; set; }
        public bool IsLastKnowLocation { get; set; }
        public float Altitude { get; set; }
        public float Battery { get; set; }
        public float EngineHours { get; set; }
        public Address1 Address { get; set; }
        public string Id { get; set; }
        public int Imei { get; set; }
        public int VbuNo { get; set; }
        public int UnitType { get; set; }
        public DateTime ServerDateTime { get; set; }
    }

    public class Address1
    {
        public string Country { get; set; }
        public string Province { get; set; }
        public string Municipality { get; set; }
        public string Town { get; set; }
        public string Suburb { get; set; }
        public string Street { get; set; }
        public int SpeedLimit { get; set; }
        public bool SpeedLimitVerified { get; set; }
        public int RoadClassification { get; set; }
        public int RoadCondition { get; set; }
        public bool RoughRoad { get; set; }
    }

    public class Location
    {
        public DateTime DateTime { get; set; }
        public float Longitude { get; set; }
        public float Latitude { get; set; }
        public float Speed { get; set; }
        public float Heading { get; set; }
        public float Odometer { get; set; }
        public bool IsIgnitionOn { get; set; }
        public int EventId { get; set; }
        public string DriverId { get; set; }
        public int AccidentId { get; set; }
        public bool IsLastKnowLocation { get; set; }
        public float Altitude { get; set; }
        public float Battery { get; set; }
        public float EngineHours { get; set; }
        public Address2 Address { get; set; }
        public string Id { get; set; }
        public int Imei { get; set; }
        public int VbuNo { get; set; }
        public int UnitType { get; set; }
        public DateTime ServerDateTime { get; set; }
    }

    public class Address2
    {
        public string Country { get; set; }
        public string Province { get; set; }
        public string Municipality { get; set; }
        public string Town { get; set; }
        public string Suburb { get; set; }
        public string Street { get; set; }
        public int SpeedLimit { get; set; }
        public bool SpeedLimitVerified { get; set; }
        public int RoadClassification { get; set; }
        public int RoadCondition { get; set; }
        public bool RoughRoad { get; set; }
    }
}
