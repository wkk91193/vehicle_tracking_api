﻿namespace VehicleTracking_Domain.Configuration.Interfaces
{
    public interface ICosmosDbConfiguration
    {
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
        string ContainerName { get; set; }
        string PartitionKeyPath { get; set; }
        
    }
}
