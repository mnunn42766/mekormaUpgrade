using System;

using CMS.Ecommerce;
using CMS.SiteProvider;
using CMS.Membership;

namespace APIExamples
{
    /// <summary>
    /// Holds department API examples.
    /// </summary>
    /// <pageTitle>Departments</pageTitle>
    internal class DepartmentsMain
    {
        /// <summary>
        /// Holds department API examples.
        /// </summary>
        /// <groupHeading>Departments</groupHeading>
        private class Departments
        {
            /// <heading>Creating a new department</heading>
            private void CreateDepartment()
            {
                // Creates a new department object
                DepartmentInfo newDepartment = new DepartmentInfo();

                // Sets the department properties
                newDepartment.DepartmentDisplayName = "New department";
                newDepartment.DepartmentName = "NewDepartment";
                newDepartment.DepartmentSiteID = SiteContext.CurrentSiteID;

                // Saves the department to the database
                DepartmentInfoProvider.SetDepartmentInfo(newDepartment);
            }


            /// <heading>Updating a department</heading>
            private void GetAndUpdateDepartment()
            {
                // Gets the department
                DepartmentInfo updateDepartment = DepartmentInfoProvider.GetDepartmentInfo("NewDepartment", SiteContext.CurrentSiteName);
                if (updateDepartment != null)
                {
                    // Updates the department properties
                    updateDepartment.DepartmentDisplayName = updateDepartment.DepartmentDisplayName.ToLower();

                    // Saves the changes to the database
                    DepartmentInfoProvider.SetDepartmentInfo(updateDepartment);
                }
            }


            /// <heading>Updating multiple departments</heading>
            private void GetAndBulkUpdateDepartments()
            {
                // Gets all departments whose code name starts with 'NewDepartment'
                var departments = DepartmentInfoProvider.GetDepartments().WhereStartsWith("DepartmentName", "NewDepartment");

                // Loops through the departments
                foreach (DepartmentInfo modifyDepartment in departments)
                {
                    // Updates the department properties
                    modifyDepartment.DepartmentDisplayName = modifyDepartment.DepartmentDisplayName.ToUpper();

                    // Saves the changes to the database
                    DepartmentInfoProvider.SetDepartmentInfo(modifyDepartment);
                }
            }


            /// <heading>Adding users as managers of a department</heading>
            private void AddUserToDepartment()
            {
                // Gets the department
                DepartmentInfo department = DepartmentInfoProvider.GetDepartmentInfo("NewDepartment", SiteContext.CurrentSiteName);
                if (department != null)
                {
                    // Adds the current user as a manager of the department
                    UserDepartmentInfoProvider.AddUserToDepartment(department.DepartmentID, MembershipContext.AuthenticatedUser.UserID);
                }
            }


            /// <heading>Removing users from a department</heading>
            private void RemoveUserFromDepartment()
            {
                // Gets the department
                DepartmentInfo department = DepartmentInfoProvider.GetDepartmentInfo("NewDepartment", SiteContext.CurrentSiteName);
                if (department != null)
                {
                    // Gets an object representing the relationship between the department and the current user
                    UserDepartmentInfo deleteUserDepartment = UserDepartmentInfoProvider.GetUserDepartmentInfo(department.DepartmentID, MembershipContext.AuthenticatedUser.UserID);

                    if (deleteUserDepartment != null)
                    {
                        // Removes the current user from the department
                        UserDepartmentInfoProvider.DeleteUserDepartmentInfo(deleteUserDepartment);
                    }
                }
            }


            /// <heading>Deleting a department</heading>
            private void DeleteDepartment()
            {
                // Gets the department
                DepartmentInfo deleteDepartment = DepartmentInfoProvider.GetDepartmentInfo("NewDepartment", SiteContext.CurrentSiteName);

                if (deleteDepartment != null)
                {
                    // Deletes the department
                    DepartmentInfoProvider.DeleteDepartmentInfo(deleteDepartment);
                }
            }
        }


        /// <summary>
        /// Holds department tax class API examples.
        /// </summary>
        /// <groupHeading>Department tax classes</groupHeading>
        private class DepartmentTaxClasses
        {
            /// <heading>Applying tax classes to a department</heading>
            private void AddTaxClassToDepartment()
            {
                // Gets the department
                DepartmentInfo department = DepartmentInfoProvider.GetDepartmentInfo("NewDepartment", SiteContext.CurrentSiteName);

                // Gets the tax class
                TaxClassInfo taxClass = TaxClassInfoProvider.GetTaxClassInfo("NewClass", SiteContext.CurrentSiteName);

                if ((department != null) && (taxClass != null))
                {
                    // Adds the tax class to the department
                    DepartmentTaxClassInfoProvider.AddTaxClassToDepartment(taxClass.TaxClassID, department.DepartmentID);
                }
            }


            /// <heading>Removing tax classes from a department</heading>
            private void RemoveTaxClassFromDepartment()
            {
                // Gets the department
                DepartmentInfo department = DepartmentInfoProvider.GetDepartmentInfo("NewDepartment", SiteContext.CurrentSiteName);

                // Gets the tax class
                TaxClassInfo taxClass = TaxClassInfoProvider.GetTaxClassInfo("NewClass", SiteContext.CurrentSiteName);

                if ((department != null) && (taxClass != null))
                {
                    // Gets an object representing the relationship between the department and the tax class
                    DepartmentTaxClassInfo departmentTax = DepartmentTaxClassInfoProvider.GetDepartmentTaxClassInfo(department.DepartmentID, taxClass.TaxClassID);

                    if ((departmentTax != null))
                    {
                        // Removes the tax class from the department
                        DepartmentTaxClassInfoProvider.DeleteDepartmentTaxClassInfo(departmentTax);
                    }
                }
            }
        }
    }
}
