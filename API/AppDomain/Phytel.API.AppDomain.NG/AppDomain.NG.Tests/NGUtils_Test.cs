using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.AppDomain.NG;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.AppDomain.NG.DTO;
using MongoDB.Bson;
using Phytel.API.DataDomain.Program.DTO;
using ServiceStack.Common;

namespace Phytel.API.AppDomain.NG.Tests
{
    [TestClass()]
    public class NGUtils_Test
    {
        [TestClass()]
        public class GetADModules
        {
            [TestMethod()]
            [TestCategory("NIGHT-950")]
            [TestProperty("TFS", "11456")]
            [TestProperty("Layer", "NGUtils")]
            public void Get_With_AssignTo_Id()
            {
                ObjectId guid = ObjectId.GenerateNewId();
                List<Module> list = new List<Module> {new Module {AssignToId = guid.ToString()}};
                var nList = NGUtils.GetADModules(list);

                Assert.AreEqual(guid.ToString(), ((ModuleDetail)nList[0]).AssignTo);
            }
        }

        [TestClass()]
        public class GetADActions_Test
        {
            [TestMethod()]
            [TestCategory("NIGHT-877")]
            [TestProperty("TFS", "11456")]
            [TestProperty("Layer", "NGUtils")]
            public void Get_Action_With_AssignTo_Id()
            {
                ObjectId guid = ObjectId.GenerateNewId();
                List<Actions> list = new List<Actions> {new Actions {AssignToId = guid.ToString()}};
                var nList = NGUtils.GetADActions(list);

                Assert.AreEqual(guid.ToString(), ((ActionsDetail) nList[0]).AssignTo);
            }
        }

        #region Contact Card

        [TestClass()]
        public class Contact_Test
        {

            [TestMethod]
            [ExpectedException(typeof(ArgumentNullException))]
            public void NGUtil_SetContactPreferredName_NullContactCard_Should_Throw()
            {
                NGUtils.SetContactPreferredName(null);
            }

            [TestMethod]
            public void NGUtil_SetContactPreferredName_EmptyPreferredName_Should_Set_PreferredName_To_FirstNamePlusLastName()
            {
                //Arrange            
                var contact =
                    new DTO.Contact
                    {
                        Id = "cid",
                        FirstName = "firstname",
                        LastName = "lastname",
                        PreferredName = string.Empty
                    };

                var firstlastname = contact.FirstName + " " + contact.LastName;

                //Act
                NGUtils.SetContactPreferredName(contact);

                //Assert
                Assert.IsTrue(firstlastname == contact.PreferredName);
            }

            [TestMethod]
            public void NGUtil_SetContactPreferredName_NotEmptyPreferredName_Should_Set_PreferredName_To_PreferredNamePlusLastName()
            {
                //Arrange
                var contact =
                    new DTO.Contact
                    {
                        Id = "cid",
                        FirstName = "firstname",
                        LastName = "lastname",
                        PreferredName = "preferredname"
                    };

                var preferrednamelastname = contact.PreferredName + " " + contact.LastName;

                //Act
                NGUtils.SetContactPreferredName(contact);

                //Assert
                Assert.IsTrue(preferrednamelastname == contact.PreferredName);
            }

            [TestMethod]
            public void NGUtil_SetContactPreferredName_NotEmptyPreferredName_Should_Set_PreferredName_To_PreferredName_WhenLastNameEmpty()
            {
                //Arrange
                var contact =
                    new DTO.Contact
                    {
                        Id = "cid",
                        FirstName = "System",
                        LastName = "",
                        PreferredName = "System"
                    };


                //Act
                NGUtils.SetContactPreferredName(contact);

                //Assert
                Assert.IsTrue("System" == contact.PreferredName);
            }

            [TestMethod]
            public void NGUtil_SetContactPreferredName_NotEmptyPreferredName_And_PreferredNameContainsLastName_Should_Set_PreferredName_To_PreferredName()
            {
                //Arrange
                var contact =
                    new DTO.Contact
                    {
                        Id = "cid",
                        FirstName = "System",
                        LastName = "lastname",
                        PreferredName = "preferredname" + " lastname"
                    };

                var preferrednamelastname = contact.PreferredName;

                //Act
                NGUtils.SetContactPreferredName(contact);

                //Assert
                Assert.IsTrue(preferrednamelastname == contact.PreferredName);
            }
        }
        #endregion

        [TestClass()]
        public class Medication_Test
        {
            [TestMethod]
            public void GetMedStrengthAndUnit_When_Both_Strength_And_Unit_Are_Present_Success()
            {
                //Arrange
                string strength = string.Empty;
                string unit = string.Empty;

                string strengthAndUnit = 
                    "6 [hp_C]/33.3mL; 6 [hp_C]/33.3mL; 6 [hp_X]/33.3mL; 6 [hp_X]/33.3mL; 6 [hp_X]/33.3mL";

                //Act
                NGUtils.GetMedStrengthAndUnit(strengthAndUnit,out strength,out unit);

                //Assert
                Assert.IsTrue(strength=="6; 6; 6; 6; 6" && unit == "[hp_C]/33.3mL; [hp_C]/33.3mL; [hp_X]/33.3mL; [hp_X]/33.3mL; [hp_X]/33.3mL");
                
            }

            [TestMethod]
            public void GetMedStrengthAndUnit_When_Both_Strength_And_Unit_AreNot_Present_Success()
            {
                //Arrange
                string strength = string.Empty;
                string unit = string.Empty;

                string strengthAndUnit = string.Empty;                    

                //Act
                NGUtils.GetMedStrengthAndUnit(strengthAndUnit, out strength, out unit);

                //Assert
                Assert.IsTrue(strength.IsEmpty() && unit.IsEmpty());

            }
        }




    }
}
