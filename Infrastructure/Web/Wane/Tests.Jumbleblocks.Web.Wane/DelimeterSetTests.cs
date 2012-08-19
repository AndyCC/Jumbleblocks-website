using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Jumbleblocks.Testing;
using Jumbleblocks.Web.Wane;
using Jumbleblocks.Web.Wane.ParseRules.Delimeters;
using Jumbleblocks.Web.Wane.ParseRules.Properties;
using Tests.Jumbleblocks.Web.Wane.Fakes;

namespace Tests.Jumbleblocks.Web.Wane
{
    /// <summary>
    /// Set of tests for delimeter set
    /// </summary>
    [TestClass]
    public class DelimeterSetTests
    {
        [TestMethod]
        public void IsValid_WHEN_EscapeDelimeter_Is_Null_THEN_Returns_False()
        {
            var set = new DelimeterSet();
            set.EscapeDelimeter = null;
            set.PropertiesStartDelimeter = "[";
            set.PropertiesEndDelimeter = "]";
            set.PropertySeperatorDelimeter = "|";
            set.PropertyNameValueSeperatorDelimeter = ":";
            set["Bold"] = "#b";

            set.IsValid.ShouldBeFalse();
        }

       [TestMethod]
        public void IsValid_WHEN_EscapeDelimeter_Is_EmptyString_THEN_Returns_False()
        {
            var set = new DelimeterSet();
            set.EscapeDelimeter = String.Empty;
            set.PropertiesStartDelimeter = "[";
            set.PropertiesEndDelimeter = "]";
            set.PropertySeperatorDelimeter = "|";
            set.PropertyNameValueSeperatorDelimeter = ":";
            set["Bold"] = "#b";

            set.IsValid.ShouldBeFalse();
        }

       [TestMethod]
        public void IsValid_WHEN_PropertiesStartDelimeter_Is_Null_THEN_Returns_False()
        {
            var set = new DelimeterSet();
            set.EscapeDelimeter = @"\";
            set.PropertiesStartDelimeter = null;
            set.PropertiesEndDelimeter = "]";
            set.PropertySeperatorDelimeter = "|";
            set.PropertyNameValueSeperatorDelimeter = ":";
            set["Bold"] = "#b";

            set.IsValid.ShouldBeFalse();
        }

       [TestMethod]
        public void IsValid_WHEN_PropertiesStartDelimeter_Is_EmptyString_THEN_Returns_False()
        {
            var set = new DelimeterSet();
            set.EscapeDelimeter = @"\";
            set.PropertiesStartDelimeter = String.Empty;
            set.PropertiesEndDelimeter = "]";
            set.PropertySeperatorDelimeter = "|";
            set.PropertyNameValueSeperatorDelimeter = ":";
            set["Bold"] = "#b";

            set.IsValid.ShouldBeFalse();
        }

       [TestMethod]
        public void IsValid_WHEN_PropertiesEndDelimeter_Is_Null_THEN_Returns_False()
        {
            var set = new DelimeterSet();
            set.EscapeDelimeter = @"\";
            set.PropertiesStartDelimeter = "[";
            set.PropertiesEndDelimeter = null;
            set.PropertySeperatorDelimeter = "|";
            set.PropertyNameValueSeperatorDelimeter = ":";
            set["Bold"] = "#b";

            set.IsValid.ShouldBeFalse();
        }

       [TestMethod]
        public void IsValid_WHEN_PropertiesEndDelimeter_Is_EmptyString_THEN_Returns_False()
        {
            var set = new DelimeterSet();
            set.EscapeDelimeter = @"\";
            set.PropertiesStartDelimeter = "[";
            set.PropertiesEndDelimeter = String.Empty;
            set.PropertySeperatorDelimeter = "|";
            set.PropertyNameValueSeperatorDelimeter = ":";
            set["Bold"] = "#b";

            set.IsValid.ShouldBeFalse();
        }

       [TestMethod]
        public void IsValid_WHEN_PropertySeperatorDelimeter_Is_Null_THEN_Returns_False()
        {
            var set = new DelimeterSet();
            set.EscapeDelimeter = @"\";
            set.PropertiesStartDelimeter = "[";
            set.PropertiesEndDelimeter = "]";
            set.PropertySeperatorDelimeter = null;
            set.PropertyNameValueSeperatorDelimeter = ":";
            set["Bold"] = "#b";

            set.IsValid.ShouldBeFalse();
        }

       [TestMethod]
        public void IsValid_WHEN_PropertySeperatorDelimeter_Is_EmptyString_THEN_Returns_False()
        {
            var set = new DelimeterSet();
            set.EscapeDelimeter = @"\";
            set.PropertiesStartDelimeter = "[";
            set.PropertiesEndDelimeter = "]";
            set.PropertySeperatorDelimeter = String.Empty;
            set.PropertyNameValueSeperatorDelimeter = ":";
            set["Bold"] = "#b";

            set.IsValid.ShouldBeFalse();
        }
        
       [TestMethod]
        public void IsValid_WHEN_PropertyNameValueSeperatorDelimeter_Is_Null_THEN_Returns_False()
        {
            var set = new DelimeterSet();
            set.EscapeDelimeter = @"\";
            set.PropertiesStartDelimeter = "[";
            set.PropertiesEndDelimeter = "]";
            set.PropertySeperatorDelimeter = "|";
            set.PropertyNameValueSeperatorDelimeter = null;
            set["Bold"] = "#b";

            set.IsValid.ShouldBeFalse();
        }

       [TestMethod]
        public void IsValid_WHEN_PropertyNameValueSeperatorDelimeter_Is_EmptyString_THEN_Returns_False()
        {
            var set = new DelimeterSet();
            set.EscapeDelimeter = @"\";
            set.PropertiesStartDelimeter = "[";
            set.PropertiesEndDelimeter = "]";
            set.PropertySeperatorDelimeter = "|";
            set.PropertyNameValueSeperatorDelimeter = String.Empty;
            set["Bold"] = "#b";

            set.IsValid.ShouldBeFalse();
        }

       [TestMethod]
        public void IsValid_WHEN_Does_Not_Contain_At_Least_1_Custom_Delimeter_THEN_Returns_False()
        {
            var set = new DelimeterSet();
            set.EscapeDelimeter = @"\";
            set.PropertiesStartDelimeter = "[";
            set.PropertiesEndDelimeter = "]";
            set.PropertySeperatorDelimeter = "|";
            set.PropertyNameValueSeperatorDelimeter = ":";

            set.IsValid.ShouldBeFalse();
        }

       [TestMethod]
        public void IsValid_WHEN_All_Properties_Set_And_One_Custom_Delimeter_THEN_Returns_True()
        {
            var set = new DelimeterSet();
            set.EscapeDelimeter = @"\";
            set.PropertiesStartDelimeter = "[";
            set.PropertiesEndDelimeter = "]";
            set.PropertySeperatorDelimeter = "|";
            set.PropertyNameValueSeperatorDelimeter = ":";
            set["Bold"] = "#b";

            set.IsValid.ShouldBeTrue();
        }

       [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddDelimeterParseRule_WHEN_delimeterParseRule_Is_Null_THEN_Throws_ArgumentNullException()
        {
            var set = new DelimeterSet();
            set.AddDelimeterParseRule(null);
        }

       [TestMethod]
        public void AddDelimeterParseRule_WHEN_delimeterParseRule_Name_Is_Bold_Then_Adds_Bold_To_List_Of_Delimeters()
        {
            const string boldDelimeterName = "bold";

            var set = new DelimeterSet();
            set.AddDelimeterParseRule(new BoldParseRule());
            set.HasDelimeterFor(boldDelimeterName).ShouldBeTrue();
        }

       [TestMethod]
        public void AddDelimeterParseRule_GIVEN_delimeterParseRule_Name_Is_Bold_WHEN_delimeterParseRule_Delimeter_Is_hashB_THEN_Indexer_For_Bold_Returns_hashB()
        {
            const string boldDelimeterName = "bold";
            const string delimeter = "#b";

            var set = new DelimeterSet();
            set.AddDelimeterParseRule(new BoldParseRule());
            set[boldDelimeterName].ShouldEqual(delimeter);
        }

       [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AddDelimeterParseRule_GIVEN_delimeterParseRule_Name_For_Bold_Already_Exists_WHEN_delimeterParseRule_Name_Is_Bold_THEN_Throws_InvalidOperationException()
        {
            const string boldDelimeterName = "bold";
            const string delimeter = "#b";

            var set = new DelimeterSet();
            set.AddDelimeterParseRule(new FakeParseRule(boldDelimeterName, delimeter, "B"));
            set.AddDelimeterParseRule(new FakeParseRule(boldDelimeterName, "#i", "I"));
        }

       [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AddDelimeterParseRule_GIVEN_delimeterParseRule_Delimieter_For_hashB_Already_Exists_WHEN_delimeterParseRule_Delimeter_Is_hashb_THEN_Throws_InvalidOperationException()
        {
            const string boldDelimeterName = "bold";
            const string delimeter = "#b";

            var set = new DelimeterSet();
            set.AddDelimeterParseRule(new FakeParseRule(boldDelimeterName, delimeter, "B"));
            set.AddDelimeterParseRule(new FakeParseRule("Something else", delimeter, "I"));
        }

       [TestMethod]
        public void AddGlobalPropertyParseRule_Adds_PropertyParseRule_To_GlobalList()
        {
            var propertyParseRule = new PropertyParseRule("StyleClass");

            var set = new DelimeterSet();            
            set.AddGlobalPropertyParseRule(propertyParseRule);

            set.GlobalPropertyParseRules.ShouldContain(propertyParseRule);
        }

       [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AddGlobalParseRule_GIVEN_propertyParseRule_With_StyleClass_Already_Exists_WHEN_propertyParseRule_With_propertyParseName_Added_Again_THEN_Throws_InvalidOperationException()
        {
            var propertyParseRule = new PropertyParseRule("StyleClass");

            var set = new DelimeterSet();
            set.AddGlobalPropertyParseRule(propertyParseRule);
            set.AddGlobalPropertyParseRule(propertyParseRule);
        }

    }
}
