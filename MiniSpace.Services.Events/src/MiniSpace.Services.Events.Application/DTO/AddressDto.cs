﻿using MiniSpace.Services.Events.Core.Entities;

namespace MiniSpace.Services.Events.Application.DTO
{
    public class AddressDto
    {
        public string BuildingName { get; set; }
        public string Street { get; set; }
        public string BuildingNumber { get; set; }
        public string ApartmentNumber { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        
        public AddressDto()
        {
        }
        
        public AddressDto(Address address)
        {
            BuildingName = address.BuildingName;
            Street = address.Street;
            BuildingNumber = address.BuildingNumber;
            ApartmentNumber = address.ApartmentNumber;
            City = address.City;
            ZipCode = address.ZipCode;
        }
    }
}