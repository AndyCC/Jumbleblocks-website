using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Jumbleblocks.Testing;
using Jumbleblocks.Web.Wane;
using Jumbleblocks.Web.Wane.ParseRules.Delimeters;
using Jumbleblocks.Web.Wane.ParseRules.Properties;

namespace Tests.Jumbleblocks.Web.Wane
{
    [TestClass]
    public class WaneTextParserTests
    {
        private DelimeterSet GetDefaultDelimeterSetWithBold()
        {
            var delimeterSet = new DelimeterSet();
            delimeterSet.AddDelimeterParseRule(new BoldParseRule());

            return delimeterSet;
        }

        private string DefaultBoldDelimeterValue
        {
            get
            {
                return DefaultDelimeterValues.Bold;
            }
        }

        private string DefaultItalicsDelimeterValue
        {
            get
            {
                return DefaultDelimeterValues.Italics;
            }
        }

        private string DefaultEscapteDelimeterValue
        {
            get
            {
                return DefaultDelimeterValues.Escape;
            }
        }

        private string DefaultPropertiesStartValue
        {
            get
            {
                return DefaultDelimeterValues.PropertiesStart;
            }
        }

        private string DefaultPropertiesEndValue
        {
            get
            {
                return DefaultDelimeterValues.PropertiesEnd;
            }
        }

        private string DefaultPropertyNameValueSeperatorValue
        {
            get
            {
                return DefaultDelimeterValues.PropertyNameValueSeperator;
            }
        }

        private string DefaultPropertySeperatorValue
        {
            get
            {
                return DefaultDelimeterValues.PropertySeperator;
            }
        }

       [TestMethod]
        public void Parse_WHEN_waneText_Contains_Text_For_Bold_Text_Bold_THEN_Returns_3_Tokens()
        {
            const string SomeText = "Some Text";
            DelimeterSet delimeterSet = GetDefaultDelimeterSetWithBold();

            string waneText = String.Format("{0}{1}{0}", DefaultBoldDelimeterValue, SomeText);

            var parser = new WaneTextParser(delimeterSet);
            IEnumerable<Token> tokens = parser.ParseText(waneText);
            
            tokens.Count().ShouldEqual(3, "Should be 3 Tokens");
        }

       [TestMethod]
        public void Parse_WHEN_waneText_Contains_Text_For_Bold_Text_Bold_THEN_First_Token_Returned_Should_Be_Bold()
        {
            const string SomeText = "Some Text";
            DelimeterSet delimeterSet = GetDefaultDelimeterSetWithBold();

            string waneText = String.Format("{0}{1}{0}", DefaultBoldDelimeterValue, SomeText);
            
            var parser = new WaneTextParser(delimeterSet);
            IEnumerable<Token> tokens = parser.ParseText(waneText);

            Token firstToken = tokens.First();
            firstToken.TokenType.ShouldEqual(TokenType.Delimiter, "Expected Token Type of Delimeter");
            firstToken.Text.ShouldEqual(DefaultDelimeterValues.Bold, String.Format("Expected text '{0}'", DefaultDelimeterValues.Bold));
        }

       [TestMethod]
        public void Parse_WHEN_waneText_Contains_Text_For_Bold_Text_Bold_THEN_Second_Token_Returned_Should_Be_Text()
        {
            const string SomeText = "Some Text";
            DelimeterSet delimeterSet = GetDefaultDelimeterSetWithBold();


            string waneText = String.Format("{0}{1}{0}", DefaultBoldDelimeterValue, SomeText);

            var parser = new WaneTextParser(delimeterSet);
            IEnumerable<Token> tokens = parser.ParseText(waneText);

            Token secondToken = tokens.ToList().ElementAt(1);
            secondToken.TokenType.ShouldEqual(TokenType.Text, "Expected Token Type of Text");
            secondToken.Text.ShouldEqual(SomeText, String.Format("Expected text '{0}'", SomeText));
        }

       [TestMethod]
        public void Parse_WHEN_waneText_Contains_Text_For_Bold_Text_Bold_THEN_Third_Token_Returned_Should_Be_Bold()
        {
            const string SomeText = "Some Text";
            DelimeterSet delimeterSet = GetDefaultDelimeterSetWithBold();


            string waneText = String.Format("{0}{1}{0}", DefaultBoldDelimeterValue, SomeText);

            var parser = new WaneTextParser(delimeterSet);
            IEnumerable<Token> tokens = parser.ParseText(waneText);

            Token LastToken = tokens.Last();
            LastToken.TokenType.ShouldEqual(TokenType.Delimiter, "Expected Token Type of Delimeter");
            LastToken.Text.ShouldEqual(DefaultDelimeterValues.Bold, String.Format("Expected text '{0}'", DefaultDelimeterValues.Bold));
        }

       [TestMethod]
        public void Parse_WHEN_waneText_Contains_Text_For_Bold_PropertyStyleClass_Text_Bold_THEN_Returns_3_Tokens()
        {
            const string SomeText = "Some Text";
            const string PropertyName = "StyleClass";
            const string PropertyValue = "ABC";

            DelimeterSet delimeterSet = GetDefaultDelimeterSetWithBold();
            delimeterSet.AddGlobalPropertyParseRule(new PropertyParseRule(PropertyName, "class"));
  
            string waneText = String.Format("{0}{1}{2}{3}{4}{5}{6}{0}", DefaultBoldDelimeterValue, delimeterSet.PropertiesStartDelimeter, 
                                                                  PropertyName, delimeterSet.PropertyNameValueSeperatorDelimeter,
                                                                  PropertyValue, delimeterSet.PropertiesEndDelimeter, SomeText);

            var parser = new WaneTextParser(delimeterSet);
            IEnumerable<Token> tokens = parser.ParseText(waneText);
            
            tokens.Count().ShouldEqual(3, "Should be 3 Tokens");
        }

       [TestMethod]
        public void Parse_WHEN_waneText_Contains_Text_For_Bold_PropertyStyleClass_Text_Bold_THEN_First_Token_Is_Bold_And_Contains_StyleClass_Property_As_Specified()
        {
            const string SomeText = "Some Text";
            const string PropertyName = "StyleClass";
            const string PropertyValue = "ABC";

            DelimeterSet delimeterSet = GetDefaultDelimeterSetWithBold();
            delimeterSet.AddGlobalPropertyParseRule(new PropertyParseRule(PropertyName, "class"));

            string waneText = String.Format("{0}{1}{2}{3}{4}{5}{6}{0}", DefaultBoldDelimeterValue, delimeterSet.PropertiesStartDelimeter,
                                                                  PropertyName, delimeterSet.PropertyNameValueSeperatorDelimeter,
                                                                  PropertyValue, delimeterSet.PropertiesEndDelimeter, SomeText);
            
            var parser = new WaneTextParser(delimeterSet);

            IEnumerable<Token> tokens = parser.ParseText(waneText);             
            
            Token firstToken = tokens.First();
            firstToken.TokenType.ShouldEqual(TokenType.Delimiter, "Expected Token Type of Delimeter");
            firstToken.Text.ShouldEqual(DefaultDelimeterValues.Bold, String.Format("Expected text '{0}'", DefaultDelimeterValues.Bold));

            firstToken.HasProperty(PropertyName).ShouldBeTrue(String.Format("Does not have property '{0}'", PropertyName));
            firstToken[PropertyName].ShouldEqual(PropertyValue, String.Format("Property With Name '{0}' does not have the correct value", PropertyName));
        }

       [TestMethod]
        public void Parse_WHEN_waneText_Contains_Text_For_Bold_PropertyStyleClass_Text_Bold_THEN_Second_Token_Is_Text()
        {
            const string SomeText = "Some Text";
            const string PropertyName = "StyleClass";
            const string PropertyValue = "ABC";

            DelimeterSet delimeterSet = GetDefaultDelimeterSetWithBold();
            delimeterSet.AddGlobalPropertyParseRule(new PropertyParseRule(PropertyName, "class"));

            string waneText = String.Format("{0}{1}{2}{3}{4}{5}{6}{0}", DefaultBoldDelimeterValue, delimeterSet.PropertiesStartDelimeter,
                                                                  PropertyName, delimeterSet.PropertyNameValueSeperatorDelimeter,
                                                                  PropertyValue, delimeterSet.PropertiesEndDelimeter, SomeText);

            var parser = new WaneTextParser(delimeterSet);

            IEnumerable<Token> tokens = parser.ParseText(waneText);

            Token secondToken = tokens.ToList().ElementAt(1);
            secondToken.TokenType.ShouldEqual(TokenType.Text, "Expected Token Type of Text");
            secondToken.Text.ShouldEqual(SomeText, String.Format("Expected text '{0}'", SomeText));
        }

       [TestMethod]
        public void Parse_WHEN_waneText_Contains_Text_For_Bold_PropertyStyleClass_Text_Bold_THEN_Third_Token_Is_Bold()
        {
            const string SomeText = "Some Text";
            const string PropertyName = "StyleClass";
            const string PropertyValue = "ABC";

            DelimeterSet delimeterSet = GetDefaultDelimeterSetWithBold();
            delimeterSet.AddGlobalPropertyParseRule(new PropertyParseRule(PropertyName, "class"));

            string waneText = String.Format("{0}{1}{2}{3}{4}{5}{6}{0}", DefaultBoldDelimeterValue, delimeterSet.PropertiesStartDelimeter,
                                                                  PropertyName, delimeterSet.PropertyNameValueSeperatorDelimeter,
                                                                  PropertyValue, delimeterSet.PropertiesEndDelimeter, SomeText);

            var parser = new WaneTextParser(delimeterSet);

            IEnumerable<Token> tokens = parser.ParseText(waneText);

            Token LastToken = tokens.Last();
            LastToken.TokenType.ShouldEqual(TokenType.Delimiter, "Expected Token Type of Delimeter");
            LastToken.Text.ShouldEqual(DefaultDelimeterValues.Bold, String.Format("Expected text '{0}'", DefaultDelimeterValues.Bold));
        }

       [TestMethod]
        public void Parse_WHEN_waneText_Contains_Text_For_Bold_Two_Properties_Text_Bold_THEN_Returns_3_Tokens()
        {
            const string SomeText = "Some Text";
            const string FirstPropertyName = "StyleClass";
            const string FirstPropertyValue = "ABC";
            const string SecondPropertyName = "Action";
            const string SecondPropertyValue = "Something";

            DelimeterSet delimeterSet = GetDefaultDelimeterSetWithBold();
            delimeterSet.AddGlobalPropertyParseRule(new PropertyParseRule(FirstPropertyName, "class"));
            delimeterSet.AddGlobalPropertyParseRule(new PropertyParseRule(SecondPropertyName, "action"));

            string waneText = String.Format("{0}{1}{2}{3}{4}{5}{6}{3}{7}{8}{9}{0}", DefaultBoldDelimeterValue, delimeterSet.PropertiesStartDelimeter,
                                                                  FirstPropertyName, delimeterSet.PropertyNameValueSeperatorDelimeter,
                                                                  FirstPropertyValue, delimeterSet.PropertySeperatorDelimeter,
                                                                  SecondPropertyName, SecondPropertyValue,
                                                                  delimeterSet.PropertiesEndDelimeter, SomeText);
            
            var parser = new WaneTextParser(delimeterSet);
            IEnumerable<Token> tokens = parser.ParseText(waneText);
     
            tokens.Count().ShouldEqual(3, "Incorrect number of tokens returned");
        }

       [TestMethod]
        public void Parse_WHEN_waneText_Contains_Text_For_Bold_Two_Properties_Text_Bold_THEN_First_Token_Contains_Two_Properties()
        {
            const string SomeText = "Some Text";
            const string FirstPropertyName = "StyleClass";
            const string FirstPropertyValue = "ABC";
            const string SecondPropertyName = "Action";
            const string SecondPropertyValue = "Something";

            DelimeterSet delimeterSet = GetDefaultDelimeterSetWithBold();
            delimeterSet.AddGlobalPropertyParseRule(new PropertyParseRule(FirstPropertyName, "class"));
            delimeterSet.AddGlobalPropertyParseRule(new PropertyParseRule(SecondPropertyName, "action"));

            string waneText = String.Format("{0}{1}{2}{3}{4}{5}{6}{3}{7}{8}{9}{0}", DefaultBoldDelimeterValue, delimeterSet.PropertiesStartDelimeter,
                                                                  FirstPropertyName, delimeterSet.PropertyNameValueSeperatorDelimeter,
                                                                  FirstPropertyValue, delimeterSet.PropertySeperatorDelimeter,
                                                                  SecondPropertyName, SecondPropertyValue,
                                                                  delimeterSet.PropertiesEndDelimeter, SomeText);

            var parser = new WaneTextParser(delimeterSet);
            IEnumerable<Token> tokens = parser.ParseText(waneText);
         
            Token firstToken = tokens.First();
            firstToken.TokenType.ShouldEqual(TokenType.Delimiter, "Expected Token Type of Delimeter");
            firstToken.Text.ShouldEqual(DefaultDelimeterValues.Bold, String.Format("Expected text '{0}'", DefaultDelimeterValues.Bold));

            firstToken.HasProperty(FirstPropertyName).ShouldBeTrue(String.Format("Does not have property '{0}'", FirstPropertyName));
            firstToken[FirstPropertyName].ShouldEqual(FirstPropertyValue, String.Format("Property With Name '{0}' does not have the correct value", FirstPropertyName));

            firstToken.HasProperty(SecondPropertyName).ShouldBeTrue(String.Format("Does not have property '{0}'", SecondPropertyName));
            firstToken[SecondPropertyName].ShouldEqual(SecondPropertyValue, String.Format("Property With Name '{0}' does not have the correct value", SecondPropertyName));
        }

       [TestMethod]
        public void Parse_WHEN_waneText_Contains_Text_For_bold_text_bold_text_bold_text_bold_THEN_Returns_7_Tokens()
        {
            const string SomeText = "Some Text";
            DelimeterSet delimeterSet = GetDefaultDelimeterSetWithBold();

            string waneText = String.Format("{0}{1}{0}{1}{0}{1}{0}", DefaultBoldDelimeterValue, SomeText);

            var parser = new WaneTextParser(delimeterSet);
            IEnumerable<Token> tokens = parser.ParseText(waneText);

            tokens.Count().ShouldEqual(7, "Should be 7 Tokens");
        }

       [TestMethod]
        public void Parse_WHEN_waneText_Contains_Text_For_bold_with_properties_text_bold_text_bold_text_bold_THEN_Returns_7_Tokens()
        {
            const string SomeText = "Some Text";
            const string PropertyName = "StyleClass";
            const string PropertyValue = "ABC";

            DelimeterSet delimeterSet = GetDefaultDelimeterSetWithBold();
            delimeterSet.AddGlobalPropertyParseRule(new PropertyParseRule(PropertyName, "class"));

            string waneText = String.Format("{0}{1}{2}{3}{4}{5}{6}{0}{6}{0}{6}{0}", DefaultBoldDelimeterValue, delimeterSet.PropertiesStartDelimeter,
                                                                  PropertyName, delimeterSet.PropertyNameValueSeperatorDelimeter,
                                                                  PropertyValue, delimeterSet.PropertiesEndDelimeter, SomeText);

            var parser = new WaneTextParser(delimeterSet);
            IEnumerable<Token> tokens = parser.ParseText(waneText);

            tokens.Count().ShouldEqual(7, "Should be 7 Tokens");
        }

       [TestMethod]
        public void Parse_WHEN_waneText_Contains_Text_For_bold_text_propertySyntax_bold_THEN_returns_3_Tokens()
        {             
            DelimeterSet delimeterSet = GetDefaultDelimeterSetWithBold();
            delimeterSet.AddGlobalPropertyParseRule(new StyleClassParseRule());

            string someText = String.Format("Some Text{0}StyleClass{1}ABC{2}", delimeterSet.PropertiesStartDelimeter, delimeterSet.PropertyNameValueSeperatorDelimeter, delimeterSet.PropertiesEndDelimeter);

            string waneText = String.Format("{0}{1}{0}", DefaultBoldDelimeterValue, someText);

            var parser = new WaneTextParser(delimeterSet);
            IEnumerable<Token> tokens = parser.ParseText(waneText);

            tokens.Count().ShouldEqual(3, "Should be 3 Tokens");
        }

       [TestMethod]
        public void Parse_WHEN_waneText_Contains_Text_For_bold_text_propertySyntax_bold_THEN_Second_Token_Is_Text_And_Contains_Text_Including_PropertySyntax()
        {
            DelimeterSet delimeterSet = GetDefaultDelimeterSetWithBold();
            delimeterSet.AddGlobalPropertyParseRule(new StyleClassParseRule());

            string someText = String.Format("Some Text{0}StyleClass{1}ABC{2}", delimeterSet.PropertiesStartDelimeter, delimeterSet.PropertyNameValueSeperatorDelimeter, delimeterSet.PropertiesEndDelimeter);

            string waneText = String.Format("{0}{1}{0}", DefaultBoldDelimeterValue, someText);

            var parser = new WaneTextParser(delimeterSet);
            IEnumerable<Token> tokens = parser.ParseText(waneText);

            Token secondToken = tokens.ToList().ElementAt(1);
            secondToken.TokenType.ShouldEqual(TokenType.Text, "token type should be text");
            secondToken.Text.ShouldEqual(someText, "Text token does not contain correct text");
        }

       [TestMethod]
        public void Parse_WHEN_waneText_Contains_Text_Which_Is_Property_Syntax_THEN_Returns_List_Of_1_Text_Token()
        {
            DelimeterSet delimeterSet = GetDefaultDelimeterSetWithBold();
            delimeterSet.AddGlobalPropertyParseRule(new StyleClassParseRule());

            string waneText = String.Format("Some Text{0}StyleClass{1}ABC{2}", delimeterSet.PropertiesStartDelimeter, delimeterSet.PropertyNameValueSeperatorDelimeter, delimeterSet.PropertiesEndDelimeter);

            var parser = new WaneTextParser(delimeterSet);
            IEnumerable<Token> tokens = parser.ParseText(waneText);

            tokens.Count().ShouldEqual(1, "Should only be 1 token");
            tokens.First().TokenType.ShouldEqual(TokenType.Text, "Should be text token type");
            tokens.First().Text.ShouldEqual(waneText, "Text is not correct");
        }

       [TestMethod]
        public void Parse_WHEN_waneText_Contains_Text_For_Bold_Italic_Text_Italic_Bold_THEN_Returns_5_Tokens()
        {
            const string SomeText = "Some Text";
            DelimeterSet delimeterSet = GetDefaultDelimeterSetWithBold();
            delimeterSet.AddDelimeterParseRule(new ItalicsParseRule());

            string waneText = String.Format("{0}{1}{2}{1}{0}", DefaultBoldDelimeterValue, DefaultItalicsDelimeterValue, SomeText);

            var parser = new WaneTextParser(delimeterSet);
            IEnumerable<Token> tokens = parser.ParseText(waneText);

            tokens.Count().ShouldEqual(5, "Should be 5 Tokens");
        }

       [TestMethod]
        public void Parse_WHEN_waneText_Contains_Text_For_Bold_Italic_Text_Italic_Bold_THEN_Returns_Correct_Tokens_In_Correct_Order()
        {
            const string SomeText = "Some Text";
            DelimeterSet delimeterSet = GetDefaultDelimeterSetWithBold();
            delimeterSet.AddDelimeterParseRule(new ItalicsParseRule());

            string waneText = String.Format("{0}{1}{2}{1}{0}", DefaultBoldDelimeterValue, DefaultItalicsDelimeterValue, SomeText);

            var parser = new WaneTextParser(delimeterSet);
            List<Token> tokens = parser.ParseText(waneText).ToList();

            tokens[0].TokenType.ShouldEqual(TokenType.Delimiter, "Should be delimeter token type");
            tokens[0].Text.ShouldEqual(DefaultBoldDelimeterValue, String.Format("Expected text '{0}'", DefaultDelimeterValues.Bold));

            tokens[1].TokenType.ShouldEqual(TokenType.Delimiter, "Should be delimeter token type");
            tokens[1].Text.ShouldEqual(DefaultItalicsDelimeterValue, String.Format("Expected text '{0}'", DefaultDelimeterValues.Italics));

            tokens[2].TokenType.ShouldEqual(TokenType.Text, "Should be text token type");
            tokens[2].Text.ShouldEqual(SomeText, "Text token does not contain correct text");

            tokens[3].TokenType.ShouldEqual(TokenType.Delimiter, "Should be delimeter token type");
            tokens[3].Text.ShouldEqual(DefaultItalicsDelimeterValue, String.Format("Expected text '{0}'", DefaultDelimeterValues.Italics));

            tokens[4].TokenType.ShouldEqual(TokenType.Delimiter, "Should be delimeter token type");
            tokens[4].Text.ShouldEqual(DefaultBoldDelimeterValue, String.Format("Expected text '{0}'", DefaultDelimeterValues.Bold));
        }


       [TestMethod]
        public void Parse_WHEN_waneText_Contains_Text_For_Bold_With_Properties_Italic_Text_Italic_Bold_THEN_Returns_5_Tokens_With_The_First_Bold_Token_Containing_Its_Properties()
        {
            const string SomeText = "Some Text";
            const string PropertyName = "StyleClass";
            const string PropertyValue = "ABC";

            DelimeterSet delimeterSet = GetDefaultDelimeterSetWithBold();
            delimeterSet.AddDelimeterParseRule(new ItalicsParseRule());
            delimeterSet.AddGlobalPropertyParseRule(new PropertyParseRule(PropertyName, "class"));

            string vPropertiesText = String.Format("{0}{1}{2}{3}{4}", DefaultDelimeterValues.PropertiesStart, PropertyName, DefaultDelimeterValues.PropertyNameValueSeperator, PropertyValue, DefaultDelimeterValues.PropertiesEnd);
            string waneText = String.Format("{0}{1}{2}{3}{2}{0}", DefaultBoldDelimeterValue ,vPropertiesText, DefaultItalicsDelimeterValue, SomeText);

            var parser = new WaneTextParser(delimeterSet);
            var tokens = parser.ParseText(waneText);

            tokens.Count().ShouldEqual(5, "Should be 5 Tokens");

            var boldToken = tokens.First();
            boldToken.TokenType.ShouldEqual(TokenType.Delimiter, "Expected delimeter token type on first token");
            boldToken.Text.ShouldEqual(DefaultBoldDelimeterValue, "Text is not default bold delimeter");

            boldToken.HasProperty(PropertyName).ShouldBeTrue(String.Format("Does not have property '{0}'", PropertyName));
            boldToken[PropertyName].ShouldEqual(PropertyValue, String.Format("Property '{0}' has incorrect value", PropertyName));
        }


       [TestMethod]
        public void Parse_WHEN_waneText_Contains_Text_For_Bold_Italic_With_Properties_Text_Italic_Bold_THEN_Returns_5_Tokens_With_The_First_Italics_Token_Containing_Its_Properties()
        {
            const string SomeText = "Some Text";
            const string PropertyName = "StyleClass";
            const string PropertyValue = "ABC";

            DelimeterSet delimeterSet = GetDefaultDelimeterSetWithBold();
            delimeterSet.AddDelimeterParseRule(new ItalicsParseRule());
            delimeterSet.AddGlobalPropertyParseRule(new PropertyParseRule(PropertyName, "class"));

            string vPropertiesText = String.Format("{0}{1}{2}{3}{4}", DefaultDelimeterValues.PropertiesStart, PropertyName, DefaultDelimeterValues.PropertyNameValueSeperator, PropertyValue, DefaultDelimeterValues.PropertiesEnd);
            string waneText = String.Format("{0}{1}{2}{3}{1}{0}", DefaultBoldDelimeterValue, DefaultItalicsDelimeterValue, vPropertiesText, SomeText);

            var parser = new WaneTextParser(delimeterSet);
            var tokens = parser.ParseText(waneText).ToList();

            tokens.Count().ShouldEqual(5, "Should be 5 Tokens");

            var italicsToken = tokens[1];
            italicsToken.TokenType.ShouldEqual(TokenType.Delimiter, "Expected delimeter token type on first token");
            italicsToken.Text.ShouldEqual(DefaultItalicsDelimeterValue, "Text is not default italics delimeter");

            italicsToken.HasProperty(PropertyName).ShouldBeTrue(String.Format("Does not have property '{0}'", PropertyName));
            italicsToken[PropertyName].ShouldEqual(PropertyValue, String.Format("Property '{0}' has incorrect value", PropertyName));
        }
        
       [TestMethod]
        public void Parse_GIVEN_DelimeterSet_Has_Rule_For_Bold_WHEN_waneText_Contains_Text_For_Bold_Text_THEN_Returns_2_Text_Tokens_With_Warning_On_Text_Token_Representing_Bold()
        {
            const string Text = "ABC";

            DelimeterSet delimeterSet = GetDefaultDelimeterSetWithBold();

            string waneText = String.Format("{0}{1}", DefaultBoldDelimeterValue, Text);

            var parser = new WaneTextParser(delimeterSet);
            var tokens = parser.ParseText(waneText).ToList();

            tokens.Count.ShouldEqual(2, "Should be 2 token");

            var boldTextToken = tokens.First();
            boldTextToken.TokenType.ShouldEqual(TokenType.Text, "Token for 'bold' should be text as no ending delimeter");
            boldTextToken.Text.ShouldEqual(DefaultBoldDelimeterValue, "text is incorrect on token for 'bold'");
            boldTextToken.Warnings.ShouldContain("Delimeter start does not have a matching end delimeter, or the actual start delimeter has been escaped");

            var textToken = tokens.Last();
            textToken.TokenType.ShouldEqual(TokenType.Text, "Text token has incorrect type");
            textToken.Text.ShouldEqual(Text, "Text token text is incorrect");
        }

       [TestMethod]
        public void Parse_GIVEN_DelimeterSet_Has_Rule_For_Bold_WHEN_waneText_Contains_Text_For_Escape_Bold_Text_Returns_One_Token_For_Text_Containing_BoldTokenText_And_Text()
        {
            const string Text = "ABC";
            string expectedText = String.Format("{0}{1}", DefaultBoldDelimeterValue, Text);

            DelimeterSet delimeterSet = GetDefaultDelimeterSetWithBold();

            string waneText = String.Format("{0}{1}", DefaultEscapteDelimeterValue, expectedText);

            var parser = new WaneTextParser(delimeterSet);
            var tokens = parser.ParseText(waneText).ToList();

            tokens.Count.ShouldEqual(1, "Should be only 1 token");

            var token = tokens.First();
            token.TokenType.ShouldEqual(TokenType.Text, "Incorrect token type");
            token.Text.ShouldEqual(expectedText, "Text not as expected");
        }

       [TestMethod]
        public void Parse_GIVEN_DelimeterSet_Has_Rule_For_Bold_WHEN_waneText_Contains_Text_For_Escape_Bold_Text_Bold_Returns_Two_Tokens_Second_Token_For_Bold_Is_Text_With_Warning()
        {
            const string Text = "ABC";
            string expectedText = String.Format("{0}{1}", DefaultBoldDelimeterValue, Text);

            DelimeterSet delimeterSet = GetDefaultDelimeterSetWithBold();

            string waneText = String.Format("{0}{1}{2}", DefaultEscapteDelimeterValue, expectedText, DefaultBoldDelimeterValue);

            var parser = new WaneTextParser(delimeterSet);
            var tokens = parser.ParseText(waneText).ToList();

            tokens.Count.ShouldEqual(2, "Should be only 1 token");

            var firstToken = tokens.First();
            firstToken.TokenType.ShouldEqual(TokenType.Text, "Incorrect token type");
            firstToken.Text.ShouldEqual(expectedText, "Text not as expected");

            var secondToken = tokens.Last();
            secondToken.TokenType.ShouldEqual(TokenType.Text, "Token for 'bold' should be text as no ending delimeter, and starting 'bold' delimeter is escaped");
            secondToken.Text.ShouldEqual(DefaultBoldDelimeterValue, "text is incorrect on token for 'bold'");
            secondToken.Warnings.ShouldContain("Delimeter start does not have a matching end delimeter, or the actual start delimeter has been escaped");
        }

       [TestMethod]
        public void Parse_GIVEN_DelimeterSet_Has_Rule_For_Bold_WHEN_waneText_Contains_Text_For_Bold_StartProperties_Text_Bold_THEN_Returns_3_Tokens_With_PropertiesStart_Part_Of_Text_Token_And_Warning_On_First_Bold()
        {
            const string Text = "ABC";
            string expectedText = String.Format("{0}{1}", DefaultPropertiesStartValue, Text);

            DelimeterSet delimeterSet = GetDefaultDelimeterSetWithBold();

            string waneText = String.Format("{0}{1}{0}", DefaultBoldDelimeterValue, expectedText);

            var parser = new WaneTextParser(delimeterSet);
            var tokens = parser.ParseText(waneText).ToList();

            tokens.Count.ShouldEqual(3, "Should be only 1 token");

            var firstToken = tokens[0];
            firstToken.TokenType.ShouldEqual(TokenType.Delimiter, "Expected delimeter token type on first token");
            firstToken.Text.ShouldEqual(DefaultBoldDelimeterValue, "Text is not default bold delimeter");
            firstToken.Warnings.ShouldContain("Properties start delimeter found, but could not find ending properties delimeter.");

            var secondToken = tokens[1];
            secondToken.TokenType.ShouldEqual(TokenType.Text, "Incorrect token type");
            secondToken.Text.ShouldEqual(expectedText, "Text not as expected");

            var thirdToken = tokens[2];
            thirdToken.TokenType.ShouldEqual(TokenType.Delimiter, "Expected delimeter token type on third token");
            thirdToken.Text.ShouldEqual(DefaultBoldDelimeterValue, "Text is not default bold delimeter");
        }
                
       [TestMethod]
        public void Parse_GIVEN_DelimeterSet_Has_Rule_For_Bold_WHEN_waneText_Contains_Text_For_Bold_StartProperties_EndProperties_Text_Bold_THEN_Returns_3_Tokens_With_PropertiesStart_And_PropertiesEnd_Not_Included_In_Text_Token_And_Warning_On_First_Bold()
        {
            const string Text = "ABC";

            DelimeterSet delimeterSet = GetDefaultDelimeterSetWithBold();

            string waneText = String.Format("{0}{1}{2}{3}{0}", DefaultBoldDelimeterValue, DefaultPropertiesStartValue, DefaultPropertiesEndValue, Text);

            var parser = new WaneTextParser(delimeterSet);
            var tokens = parser.ParseText(waneText).ToList();

            tokens.Count.ShouldEqual(3, "Should be only 1 token");

            var firstToken = tokens[0];
            firstToken.TokenType.ShouldEqual(TokenType.Delimiter, "Expected delimeter token type on first token");
            firstToken.Text.ShouldEqual(DefaultBoldDelimeterValue, "Text is not default bold delimeter");
            firstToken.Warnings.ShouldContain("Properties declaration contains no key value pairs. Could not construct properties.");

            var secondToken = tokens[1];
            secondToken.TokenType.ShouldEqual(TokenType.Text, "Incorrect token type");
            secondToken.Text.ShouldEqual(Text, "Text not as expected");

            var thirdToken = tokens[2];
            thirdToken.TokenType.ShouldEqual(TokenType.Delimiter, "Expected delimeter token type on third token");
            thirdToken.Text.ShouldEqual(DefaultBoldDelimeterValue, "Text is not default bold delimeter");
        }

       [TestMethod]
        public void Parse_GIVEN_DelimeterSet_Has_Rule_For_Bold_WHEN_waneText_Contains_Text_For_Bold_StartProperties_PropertyName_EndProperties_Text_Bold_THEN_Returns_3_Tokens_With_PropertiesStart_And_PropertiesEnd_Not_Included_In_Text_Token_And_Warning_On_First_Bold()
        {
            const string Text = "ABC";
            const string PropertyName = "StyleClass";

            DelimeterSet delimeterSet = GetDefaultDelimeterSetWithBold();
            delimeterSet.AddGlobalPropertyParseRule(new PropertyParseRule(PropertyName, "class"));

            string waneText = String.Format("{0}{1}{2}{3}{4}{0}", DefaultBoldDelimeterValue, DefaultPropertiesStartValue, PropertyName, DefaultPropertiesEndValue, Text);

            var parser = new WaneTextParser(delimeterSet);
            var tokens = parser.ParseText(waneText).ToList();

            tokens.Count.ShouldEqual(3, "Should be only 1 token");

            var firstToken = tokens[0];
            firstToken.TokenType.ShouldEqual(TokenType.Delimiter, "Expected delimeter token type on first token");
            firstToken.Text.ShouldEqual(DefaultBoldDelimeterValue, "Text is not default bold delimeter");
            firstToken.Warnings.ShouldContain("Properties declaration contains no value for the key " + PropertyName);

            var secondToken = tokens[1];
            secondToken.TokenType.ShouldEqual(TokenType.Text, "Incorrect token type");
            secondToken.Text.ShouldEqual(Text, "Text not as expected");

            var thirdToken = tokens[2];
            thirdToken.TokenType.ShouldEqual(TokenType.Delimiter, "Expected delimeter token type on third token");
            thirdToken.Text.ShouldEqual(DefaultBoldDelimeterValue, "Text is not default bold delimeter");
        }

       [TestMethod]
        public void Parse_GIVEN_DelimeterSet_Has_Rule_For_Bold_WHEN_waneText_Contains_Text_For_Bold_StartProperties_PropertyName_PropertyNameValueSeperator_EndProperties_Text_Bold_THEN_Returns_3_Tokens_With_PropertiesStart_And_PropertiesEnd_Not_Included_In_Text_Token_And_Warning_On_First_Bold()
        {
            const string Text = "ABC";
            const string PropertyName = "StyleClass";

            DelimeterSet delimeterSet = GetDefaultDelimeterSetWithBold();
            delimeterSet.AddGlobalPropertyParseRule(new PropertyParseRule(PropertyName, "class"));

            string waneText = String.Format("{0}{1}{2}{3}{4}{5}{0}", DefaultBoldDelimeterValue, DefaultPropertiesStartValue, PropertyName, DefaultPropertyNameValueSeperatorValue, DefaultPropertiesEndValue, Text);

            var parser = new WaneTextParser(delimeterSet);
            var tokens = parser.ParseText(waneText).ToList();

            tokens.Count.ShouldEqual(3, "Should be only 1 token");

            var firstToken = tokens[0];
            firstToken.TokenType.ShouldEqual(TokenType.Delimiter, "Expected delimeter token type on first token");
            firstToken.Text.ShouldEqual(DefaultBoldDelimeterValue, "Text is not default bold delimeter");
            firstToken.Warnings.ShouldContain("Properties declaration contains no value for the key " + PropertyName);

            var secondToken = tokens[1];
            secondToken.TokenType.ShouldEqual(TokenType.Text, "Incorrect token type");
            secondToken.Text.ShouldEqual(Text, "Text not as expected");

            var thirdToken = tokens[2];
            thirdToken.TokenType.ShouldEqual(TokenType.Delimiter, "Expected delimeter token type on third token");
            thirdToken.Text.ShouldEqual(DefaultBoldDelimeterValue, "Text is not default bold delimeter");
        }

       [TestMethod]
        public void Parse_GIVEN_DelimeterSet_Has_Rule_For_Bold_WHEN_waneText_Contains_Text_For_Bold_StartProperties_PropertyName_PropertyNameValueSeperator_PropertySeperator_PropertyName_EndProperties_Text_Bold_THEN_Returns_3_Tokens_With_Warning_On_First_Bold_And_Bold_Has_First_Property()
        {
            const string Text = "ABC";
            const string PropertyName = "StyleClass";
            const string PropertyValue = "class1";
            const string PropertyTwoName = "Action";

            DelimeterSet delimeterSet = GetDefaultDelimeterSetWithBold();
            delimeterSet.AddGlobalPropertyParseRule(new PropertyParseRule(PropertyName, "class"));

            string propertyOne = String.Format("{0}{1}{2}", PropertyName, DefaultPropertyNameValueSeperatorValue, PropertyValue);
            string waneText = String.Format("{0}{1}{2}{3}{4}{5}{6}{0}", DefaultBoldDelimeterValue, DefaultPropertiesStartValue, propertyOne, DefaultPropertySeperatorValue, PropertyTwoName, DefaultPropertiesEndValue, Text);

            var parser = new WaneTextParser(delimeterSet);
            var tokens = parser.ParseText(waneText).ToList();
             
            tokens.Count.ShouldEqual(3, "Should be only 1 token");

            var firstToken = tokens[0];
            firstToken.TokenType.ShouldEqual(TokenType.Delimiter, "Expected delimeter token type on first token");
            firstToken.Text.ShouldEqual(DefaultBoldDelimeterValue, "Text is not default bold delimeter");
            firstToken.Warnings.ShouldContain("Properties declaration contains no value for the key " + PropertyTwoName);

            firstToken.HasProperty(PropertyName).ShouldBeTrue();
            firstToken.PropertyCount.ShouldEqual(1, "Should only be 1 property");

            var secondToken = tokens[1];
            secondToken.TokenType.ShouldEqual(TokenType.Text, "Incorrect token type");
            secondToken.Text.ShouldEqual(Text, "Text not as expected");

            var thirdToken = tokens[2];
            thirdToken.TokenType.ShouldEqual(TokenType.Delimiter, "Expected delimeter token type on third token");
            thirdToken.Text.ShouldEqual(DefaultBoldDelimeterValue, "Text is not default bold delimeter");
        }


       [TestMethod]
        public void Parse_GIVEN_DelimeterSet_Has_Rule_For_Bold_WHEN_waneText_Contains_Text_For_Bold_StartProperties_PropertyName_PropertyNameValueSeperator_PropertySeperator_PropertyName_PropertyNameValueSeperator_PropeEndProperties_Text_Bold_THEN_Returns_3_Tokens_With_Warning_On_First_Bold_And_Bold_Has_First_Property()
        {
            const string Text = "ABC";
            const string PropertyName = "StyleClass";
            const string PropertyValue = "class1";
            const string PropertyTwoName = "Action";

            DelimeterSet delimeterSet = GetDefaultDelimeterSetWithBold();
            delimeterSet.AddGlobalPropertyParseRule(new PropertyParseRule(PropertyName, "class"));
            delimeterSet.AddGlobalPropertyParseRule(new PropertyParseRule(PropertyTwoName, "action"));

            string propertyOne = String.Format("{0}{1}{2}", PropertyName, DefaultPropertyNameValueSeperatorValue, PropertyValue);
            string propertyTwo = String.Format("{0}{1}", PropertyTwoName, DefaultPropertyNameValueSeperatorValue);

            string waneText = String.Format("{0}{1}{2}{3}{4}{5}{6}{0}", DefaultBoldDelimeterValue, DefaultPropertiesStartValue, propertyOne, DefaultPropertySeperatorValue, propertyTwo, DefaultPropertiesEndValue, Text);

            var parser = new WaneTextParser(delimeterSet);
            var tokens = parser.ParseText(waneText).ToList();

            tokens.Count.ShouldEqual(3, "Should be 3 tokens");

            var firstToken = tokens[0];
            firstToken.TokenType.ShouldEqual(TokenType.Delimiter, "Expected delimeter token type on first token");
            firstToken.Text.ShouldEqual(DefaultBoldDelimeterValue, "Text is not default bold delimeter");
            firstToken.Warnings.ShouldContain("Properties declaration contains no value for the key " + PropertyTwoName);

            firstToken.HasProperty(PropertyName).ShouldBeTrue();
            firstToken.PropertyCount.ShouldEqual(1, "Should only be 1 property");

            var secondToken = tokens[1];
            secondToken.TokenType.ShouldEqual(TokenType.Text, "Incorrect token type");
            secondToken.Text.ShouldEqual(Text, "Text not as expected");

            var thirdToken = tokens[2];
            thirdToken.TokenType.ShouldEqual(TokenType.Delimiter, "Expected delimeter token type on third token");
            thirdToken.Text.ShouldEqual(DefaultBoldDelimeterValue, "Text is not default bold delimeter");
        }

       [TestMethod]
        public void Parse_GIVEN_DelimeterSet_Has_Rule_For_Bold_WHEN_waneText_Contains_Text_For_Bold_StartProperties_PropertyName_commer_PropertySeperator_PropertyName_PropertyNameValueSeperator_PropertyValue_EndProperties_Text_Bold_THEN_Returns_3_Tokens_With_One_Property_On_Bold()
        {
            const string Text = "ABC";
            const string PropertyName = "StyleClass";
            const string PropertyValue = "class1";
            const string PropertyTwoName = "Action";

            DelimeterSet delimeterSet = GetDefaultDelimeterSetWithBold();
            delimeterSet.AddGlobalPropertyParseRule(new PropertyParseRule(PropertyName, "class"));
            delimeterSet.AddGlobalPropertyParseRule(new PropertyParseRule(PropertyTwoName, "action"));

            string propertyOne = String.Format("{0}{1}{2}", PropertyName, DefaultPropertyNameValueSeperatorValue, PropertyValue);
            string propertyTwo = String.Format("{0}{1}", PropertyTwoName, DefaultPropertyNameValueSeperatorValue);

            string waneText = String.Format("{0}{1}{2}{3},{4}{5}{0}", DefaultBoldDelimeterValue, DefaultPropertiesStartValue, propertyOne, propertyTwo, DefaultPropertiesEndValue, Text);

            var parser = new WaneTextParser(delimeterSet);
            var tokens = parser.ParseText(waneText).ToList();

            tokens.Count.ShouldEqual(3, "Should be 3 tokens");

            var firstToken = tokens[0];
            firstToken.TokenType.ShouldEqual(TokenType.Delimiter, "Expected delimeter token type on first token");
            firstToken.Text.ShouldEqual(DefaultBoldDelimeterValue, "Text is not default bold delimeter");
            firstToken.HasProperty(PropertyName).ShouldBeTrue();
            firstToken.PropertyCount.ShouldEqual(1, "Should only be 1 property");

            var secondToken = tokens[1];
            secondToken.TokenType.ShouldEqual(TokenType.Text, "Incorrect token type");
            secondToken.Text.ShouldEqual(Text, "Text not as expected");

            var thirdToken = tokens[2];
            thirdToken.TokenType.ShouldEqual(TokenType.Delimiter, "Expected delimeter token type on third token");
            thirdToken.Text.ShouldEqual(DefaultBoldDelimeterValue, "Text is not default bold delimeter");
        }

       [TestMethod]
        public void Parse_GIVEN_DelimeterSet_Has_Rule_For_Bold_WHEN_waneText_Contains_Text_For_Bold_Escape_PropertyStart_PropertyName_PropertyNameValueSeperator_PropertyValue_EndProperties_Text_Bold_THEN_Returns_3_Tokens_With_Property_Declaration_In_Text()
        {
            const string Text = "ABC";
            const string PropertyName = "StyleClass";
            const string PropertyValue = "class1";

            DelimeterSet delimeterSet = GetDefaultDelimeterSetWithBold();
            delimeterSet.AddGlobalPropertyParseRule(new PropertyParseRule(PropertyName, "class"));

            string propertyText = String.Format("{0}{1}{2}{3}{4}", DefaultPropertiesStartValue, PropertyName, DefaultPropertyNameValueSeperatorValue, PropertyValue, DefaultPropertiesEndValue);
            string waneText = String.Format("{0}{1}{2}{3}{0}", DefaultBoldDelimeterValue, DefaultEscapteDelimeterValue, propertyText, Text);
            
            var parser = new WaneTextParser(delimeterSet);
            var tokens = parser.ParseText(waneText).ToList();

            tokens.Count.ShouldEqual(3, "Should be 3 tokens");

            var firstToken = tokens[0];
            firstToken.TokenType.ShouldEqual(TokenType.Delimiter, "Expected delimeter token type on first token");
            firstToken.Text.ShouldEqual(DefaultBoldDelimeterValue, "Text is not default bold delimeter");
            firstToken.PropertyCount.ShouldEqual(0, "Should be no properties");
            
            var secondToken = tokens[1];
            secondToken.TokenType.ShouldEqual(TokenType.Text, "Incorrect token type");
            secondToken.Text.ShouldEqual(String.Format("{0}{1}", propertyText, Text), "Text not as expected");

            var thirdToken = tokens[2];
            thirdToken.TokenType.ShouldEqual(TokenType.Delimiter, "Expected delimeter token type on third token");
            thirdToken.Text.ShouldEqual(DefaultBoldDelimeterValue, "Text is not default bold delimeter");
        }
        
       [TestMethod]
        public void Parse_GIVEN_DelimeterSet_Has_Rule_For_Bold_WHEN_waneText_Contains_Text_For_Bold_Text_Bold_Properties_THEN_Returns_3_Tokens_With_Warning_On_Second_Bold_And_Properties_And_Not_Made_Into_Text()
        {
            const string Text = "ABC";
            const string PropertyName = "StyleClass";
            const string PropertyValue = "class1";

            DelimeterSet delimeterSet = GetDefaultDelimeterSetWithBold();

            string propertyText = String.Format("{0}{1}{2}{3}{4}", DefaultPropertiesStartValue, PropertyName, DefaultPropertyNameValueSeperatorValue, PropertyValue, DefaultPropertiesEndValue);
            string waneText = String.Format("{0}{1}{0}{2}", DefaultBoldDelimeterValue, Text, propertyText);

            var parser = new WaneTextParser(delimeterSet);
            var tokens = parser.ParseText(waneText).ToList();

            tokens.Count.ShouldEqual(3, "Should be 3 tokens");

            var firstToken = tokens[0];
            firstToken.TokenType.ShouldEqual(TokenType.Delimiter, "Expected delimeter token type on first token");
            firstToken.Text.ShouldEqual(DefaultBoldDelimeterValue, "Text is not default bold delimeter");     

            var secondToken = tokens[1];
            secondToken.TokenType.ShouldEqual(TokenType.Text, "Incorrect token type");
            secondToken.Text.ShouldEqual(Text, "Text not as expected");

            var thirdToken = tokens[2];
            thirdToken.TokenType.ShouldEqual(TokenType.Delimiter, "Expected delimeter token type on third token");
            thirdToken.Text.ShouldEqual(DefaultBoldDelimeterValue, "Text is not default bold delimeter");
            thirdToken.PropertyCount.ShouldEqual(0, "Should be no properties");

            thirdToken.Warnings.Count().ShouldEqual(1, "Should be 1 warning");
            thirdToken.Warnings.ShouldContain("Can not put properties on closing delimeters.");
        }

       [TestMethod]
        public void Parse_GIVEN_DelimeterSet_Has_Rule_For_Bold_WHEN_waneText_Contains_Text_For_Escape_Escape_Bold_Text_Bold_THEN_Returns_Text_With_Escape_Character_Followed_By_Bold_Text_Bold()
        {
            DelimeterSet delimeterSet = GetDefaultDelimeterSetWithBold();

            string waneText = String.Format("{0}{0}{1}Text{1}", DefaultEscapteDelimeterValue, DefaultBoldDelimeterValue);

            var parser = new WaneTextParser(delimeterSet);
            var tokens = parser.ParseText(waneText).ToList();

            tokens.Count.ShouldEqual(4, "Should be 4 tokens");

            var firstToken = tokens[0];
            firstToken.TokenType.ShouldEqual(TokenType.Text, "Expected text token type on first token");
            firstToken.Text.ShouldEqual(DefaultEscapteDelimeterValue, "Text is not as expected");

            var secondToken = tokens[1];
            secondToken.TokenType.ShouldEqual(TokenType.Delimiter, "Expected delimeter token type on first token");
            secondToken.Text.ShouldEqual(DefaultBoldDelimeterValue, "Text is not default bold delimeter");

            var thirdToken = tokens[2];
            thirdToken.TokenType.ShouldEqual(TokenType.Text, "Incorrect token type");

            var fourthToken = tokens[1];
            fourthToken.TokenType.ShouldEqual(TokenType.Delimiter, "Expected delimeter token type on first token");
            fourthToken.Text.ShouldEqual(DefaultBoldDelimeterValue, "Text is not default bold delimeter");
        }

       [TestMethod]
        public void Parse_GIVEN_DelimeterSet_Has_Rule_For_Bold_WHEN_waneText_Contains_Text_For_Bold_With_Two_Properties_With_Same_Name_But_Differnt_Values_Text_Bold_THEN_Returns_3_Tokens_With_Forst_Bold_Token_Having_1_Property_With_Last_Value_And_A_Warning()
        {
            const string PropertyName = "StyleClass";
            const string PropertyValue = "ABC";
            const string PropertyValue2 = "ABC2";

            DelimeterSet delimeterSet = GetDefaultDelimeterSetWithBold();
            delimeterSet.AddGlobalPropertyParseRule(new PropertyParseRule(PropertyName, "class"));

            string propertyText = String.Format("{0}{1}{2}{3}{4}{1}{2}{5}{6}", DefaultPropertiesStartValue, PropertyName, DefaultPropertyNameValueSeperatorValue, PropertyValue, DefaultPropertySeperatorValue, PropertyValue2, DefaultPropertiesEndValue);
            string waneText = String.Format("{0}{1}Text{0}", DefaultBoldDelimeterValue, propertyText);

            var parser = new WaneTextParser(delimeterSet);
            var tokens = parser.ParseText(waneText).ToList();

            tokens.Count.ShouldEqual(3, "Should be 3 tokens");

            var firstToken = tokens.First();
            firstToken.HasProperty(PropertyName).ShouldBeTrue("Does not have property");
            firstToken[PropertyName].ShouldEqual(PropertyValue2, "Property Value is incorrect");

            firstToken.HasWarnings.ShouldBeTrue("Should have warning");
            firstToken.Warnings.ShouldContain(String.Format("Property '{0}' assigned multiple values.", PropertyName));
        }
         
       [TestMethod]
        public void Parse_GIVEN_DelimeterSet_Has_Rule_For_Bold_WHEN_waneText_Contains_Text_For_Bold_Text_Newline_Bold_THEN_Returns_4_Tokens_With_3rd_Token_Being_A_NewLine()
        {
            DelimeterSet delimeterSet = GetDefaultDelimeterSetWithBold();
            string waneText = String.Format("{0}Text{1}{0}", DefaultBoldDelimeterValue, Environment.NewLine);

            var parser = new WaneTextParser(delimeterSet);
            var tokens = parser.ParseText(waneText).ToList();
            
            tokens.Count.ShouldEqual(4, "Should be 4 tokens");

            var thirdToken = tokens[2];
            thirdToken.TokenType.ShouldEqual(TokenType.NewLine, "Is not a new line");
        }

       [TestMethod]
        public void Parse_GIVEN_DelimeterSet_Has_Rule_For_Bold_And_No_PropertyParse_Rules_WHEN_Bold_Has_A_Property_THEN_Does_Not_Add_Property_To_Bold_But_Adds_Warning()
        {
            DelimeterSet delimeterSet = GetDefaultDelimeterSetWithBold();

            
            string properties = String.Format("{0}{1}{2}{3}{4}", DefaultPropertiesStartValue, StyleClassParseRule.NameOfProperty, DefaultPropertyNameValueSeperatorValue, "ABC", DefaultPropertiesEndValue);
            string waneText = String.Format("{0}{1}Text{0}", DefaultBoldDelimeterValue, properties);

            var parser = new WaneTextParser(delimeterSet);
            var tokens = parser.ParseText(waneText).ToList();

            tokens.Count.ShouldEqual(3, "Should be 3 tokens");

            var boldToken = tokens.First();

            boldToken.HasProperty(StyleClassParseRule.NameOfProperty).ShouldBeFalse("Should not have property");
            boldToken.Warnings.ShouldContain(String.Format("The property '{0}' is not allowed on this delimeter.", StyleClassParseRule.NameOfProperty));
        }

       [TestMethod]
        public void Parse_WHEN_Delimieter_Is_Escaped_Twice_THEN_Does_Not_Return_Escape_Character_In_Text()
        {
            DelimeterSet delimeterSet = GetDefaultDelimeterSetWithBold();
            string waneText = String.Format("{0}{1} {0}{1}", DefaultEscapteDelimeterValue, DefaultBoldDelimeterValue);

            var parser = new WaneTextParser(delimeterSet);
            var tokens = parser.ParseText(waneText).ToList();

            tokens.Count.ShouldEqual(1, "Should be 1 tokens");

            var token = tokens.First();

            token.IsText.ShouldBeTrue();
            token.Text.ShouldEqual(String.Format("{0} {0}",DefaultBoldDelimeterValue));

         
        }


        //POSSIBLE FUTURE FUNCTIONALITY TO CORRECT BELOW

        //bold, italic, text, bold, text, italic - returns italic, bold, text, bold, text, italic - warning on opening and closing italics and bold about order
        //bold, italic[properties], text, bold, text, italic - returns italic[properties], bold, text, bold, text, italic - warning on opening and closing italics and bold about order

     
    }
}
