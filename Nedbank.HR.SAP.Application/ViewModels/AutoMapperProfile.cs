using AutoMapper;
using Nedbank.HR.SAP.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nedbank.HR.SAP.BAL.ViewModels
{
    public class AutoMapperProfile: Profile
    {
        public AutoMapperProfile()
        {
            
            CreateMap<HR_SAP_Employee, Employee>()
                .ForMember(destination =>
                destination.EmployeeNo,
                options => options.MapFrom(source => source.EmployeeNumber))

                // Employee person mappings
                .ForPath(destination =>
                destination.Person.FirstName, options =>
                options.MapFrom(source => source.EmployeeFirstname))
                .ForPath(destination =>
                destination.Person.LastName, options =>
                options.MapFrom(source => source.EmployeeSurname))
                .ForPath(destination =>
                destination.Person.NickName, options =>
                options.MapFrom(source => source.EmployeeCallName))
                .ForPath(destination =>
                destination.Person.Initials, options =>
                options.Ignore())

                //Employee job title mappings

                .ForMember(destination =>
                destination.JobTitle, options => options.MapFrom(source => new JobTitle
                {
                    Code = source.EmployeeJobTitle,
                    Description = source.EmployeeJobTitle
                }))

                // employee email and phone mappings
                .ForPath(destination =>
                destination.ContactDetails.ElectronicAddress,
                options => options.MapFrom(source =>
                new ElectronicAddress[] {
                    new ElectronicAddress { Email = source.EmployeeEmailAddress, Usage = new Usage{ } }
                }))
                .ForPath(destination =>
                destination.ContactDetails.PhoneNumber,
                options => options.MapFrom(source =>
                new PhoneNumber[]
                {
                    new PhoneNumber
                    {
                        AreaCode = string.Empty,
                        Usage = string.Empty,
                        PhoneNo = source.EmployeeCellularNumber
                    }
                }))

                // employee address mappings
                .ForPath(destination =>
                destination.Addresses,
                options => options.MapFrom(source =>
                new Address[]
                {
                    new Address
                    {
                        AddressLine1 = source.EmployeePhysicalLocation,
                        AddressLine2 = string.Empty,
                        AddressLine3 = string.Empty,
                        AddressLine4 = string.Empty,
                        City = string.Empty,
                        CountryCode = new CountryCode{ },
                        PostalCode = string.Empty,
                        Region = string.Empty,
                        SubUrb = string.Empty,
                        AddressType = new AddressType{ }
                    }
                }))

                // Branch mappings
                .ForPath(destination =>
                destination.EmployeeBranch.BranchId,
                options => options.MapFrom(source => source.LegacyBranchId))
                .ForPath(destination =>
                destination.EmployeeBranch.BranchNumber,
                options => options.MapFrom(source => source.EmployeeBranchNumber))
                .ForPath(destination =>
                destination.EmployeeBranch.BranchName,
                options => options.MapFrom(source => source.EmployeeBranchName))

                // Reporting To staff mapping
                .ForMember(destination =>
                destination.ReportToStaffNo, options => options.Ignore())
                // Cluster mapping
                .ForMember(destination =>
                destination.Cluster, options => options.MapFrom(source => source.EmployeeCluster));

            CreateMap<Employee, HR_SAP_Employee>()
                .ForMember(destination =>
                destination.EmployeeNumber, options =>
                options.MapFrom(source => source.EmployeeNo))
                .ForMember(destination =>
                destination.EmployeeFirstname, options =>
                options.MapFrom(source => source.Person.FirstName))
                .ForMember(destination =>
                destination.EmployeeSurname, options =>
                options.MapFrom(source => source.Person.LastName))
                .ForMember(destination =>
                destination.EmployeeCallName, options =>
                options.MapFrom(source => source.Person.NickName))
                .ForMember(destination =>
                destination.EmployeeJobTitle, options =>
                options.MapFrom(source => source.JobTitle.Description))
                .ForMember(destination =>
                destination.EmployeeCluster, options =>
                options.MapFrom(source => source.Cluster))
                .ForMember(destination =>
                destination.EmployeeBranchNumber, options =>
                options.MapFrom(source => source.EmployeeBranch.BranchNumber))
                .ForMember(destination =>
                destination.LegacyBranchId, options =>
                options.MapFrom(source => source.EmployeeBranch.BranchId))
                .ForMember(destination =>
                destination.EmployeeBranchName, options =>
                options.MapFrom(source => source.EmployeeBranch.BranchName))
                .ForMember(destination =>
                destination.EmployeeEmailAddress, options =>
                options.MapFrom(source => source.ContactDetails.ElectronicAddress[0].Email))
                .ForMember(destination =>
                destination.EmployeeCellularNumber, options =>
                options.MapFrom(source => source.ContactDetails.PhoneNumber[0].PhoneNo))
                .ForMember(destination =>
                destination.EmployeePhysicalLocation, options =>
                options.MapFrom(source => string.Concat(source.Addresses[0].AddressLine1)));

        }
    }
}
