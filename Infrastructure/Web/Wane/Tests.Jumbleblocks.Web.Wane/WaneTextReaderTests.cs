using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Jumbleblocks.Testing;
using Jumbleblocks.Web.Wane;
using Jumbleblocks.Web.Wane.ParseRules.Delimeters;

namespace Tests.Jumbleblocks.Web.Wane
{
    [TestClass]
    public class WaneTextReaderTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Ctor_WHEN_delimiters_Is_Null_THEN_Throw_ArgumentNullException()
        {
            var reader = new WaneTextReader(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Ctor_WHEN_delimiters_Is_Empty_Array_THEN_Throws_ArgumentException()
        {
            var reader = new WaneTextReader(new string[0]);
        }

        [TestMethod]
        public void SetText_WHEN_text_Is_Null_THEN_Sets__IsEndOfStream_To_True()
        {
            var reader = new WaneTextReader(new string[] { DefaultDelimeterValues.Bold });
            reader.SetText(null);
            reader.IsEndOfStream.ShouldBeTrue();
        }

        [TestMethod]
        public void SetText_WHEN_text_Is_EmptyString_THEN_Sets__IsEndOfStream_To_True()
        {
            var reader = new WaneTextReader(new string[] { DefaultDelimeterValues.Bold });
            reader.SetText(String.Empty);
            reader.IsEndOfStream.ShouldBeTrue();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void FindToken_GIVEN_SetText_Has_Not_Been_Called_THEN_Throws_InvalidOperationException()
        {
            var reader = new WaneTextReader(new string[] { DefaultDelimeterValues.Bold });
            reader.ReadNextToken();
        }

        [TestMethod]
        public void ReadNextToken_GIVEN_Setup_text_Is_abc_AND_delimiters_Contains_Bold_THEN_Returns_1_Token_For_The_Text()
        {
            const string text = "abc";

            var reader = new WaneTextReader(new string[] { DefaultDelimeterValues.Bold });
            reader.SetText(text);

            Token token = reader.ReadNextToken();

            token.Text.ShouldEqual(text);
            token.TokenType.ShouldEqual(TokenType.Text);
            token.CharPosition.ShouldEqual(1);
            token.LinePosition.ShouldEqual(1);
        }

        [TestMethod]
        public void ReadNextToken_GIVEN_Setup_text_Is_abc_AND_delimiters_Contains_Bold_THEN_Sets_IsEndOfText_To_True()
        {
            const string text = "abc";

            var reader = new WaneTextReader(new string[] { DefaultDelimeterValues.Bold });
            reader.SetText(text);

            reader.ReadNextToken();
            reader.IsEndOfStream.ShouldBeTrue();
        }

        [TestMethod]
        public void ReadNextToken_GIVEN_Setup_text_Is_abc_hashB_def_AND_delimiters_Contains_BOLD_WHEN_FindToken_Called_For_First_Time_THEN_Returns_Token_For_Text_abc()
        {
            const string firstTokenText = "abc";
            const string text = firstTokenText + "#bdef";

            var reader = new WaneTextReader(new string[] { DefaultDelimeterValues.Bold });
            reader.SetText(text);

            Token token = reader.ReadNextToken();

            token.Text.ShouldEqual(firstTokenText);
            token.TokenType.ShouldEqual(TokenType.Text);
            token.LinePosition.ShouldEqual(1);
            token.CharPosition.ShouldEqual(1);
        }

        [TestMethod]
        public void ReadNextToken_GIVEN_IsEndOfText_Is_True_THEN_Returns_Null()
        {
            const string text = "abc";

            var reader = new WaneTextReader(new string[] { DefaultDelimeterValues.Bold });
            reader.SetText(text);

            reader.ReadNextToken();
            reader.IsEndOfStream.ShouldBeTrue("pre check on IsEndOfText");

            reader.ReadNextToken().ShouldBeNull();
        }

        [TestMethod]
        public void ReadNextToken_GIVEN_Setup_text_Is_abc_hashB_def_AND_delimiters_Contains_BOLD_WHEN_FindToken_Called_For_First_Time_THEN_Sets_IsEndOfText_To_False()
        {
            const string firstTokenText = "abc";
            const string text = firstTokenText + "#bdef";

            var reader = new WaneTextReader(new string[] { DefaultDelimeterValues.Bold });
            reader.SetText(text);

            reader.ReadNextToken();

            reader.IsEndOfStream.ShouldBeFalse();
        }

        [TestMethod]
        public void ReadNextToken_GIVEN_Setup_text_Is_abc_hashB_def_AND_delimiters_Contains_BOLD_WHEN_FindToken_Called_For_Second_Time_THEN_Returns_Token_For_Delimeter_Bold()
        {
            const string secondTokenText = "#b";
            const string text = "abc" + secondTokenText + "def";

            var reader = new WaneTextReader(new string[] { DefaultDelimeterValues.Bold });
            reader.SetText(text);

            reader.ReadNextToken();
            Token token = reader.ReadNextToken();

            token.Text.ShouldEqual(secondTokenText);
            token.TokenType.ShouldEqual(TokenType.Delimiter);
            token.LinePosition.ShouldEqual(1);
            token.CharPosition.ShouldEqual(4);
        }

        [TestMethod]
        public void ReadNextToken_GIVEN_Setup_text_Is_abc_hashB_def_AND_delimiters_Contains_BOLD_WHEN_FindToken_Called_For_Second_Time_THEN_Sets_IsEndOfText_To_False()
        {
            const string secondTokenText = "#b";
            const string text = "abc" + secondTokenText + "def";

            var reader = new WaneTextReader(new string[] { DefaultDelimeterValues.Bold });
            reader.SetText(text);

            reader.ReadNextToken();
            reader.ReadNextToken();

            reader.IsEndOfStream.ShouldBeFalse();
        }

        [TestMethod]
        public void ReadNextToken_GIVEN_Setup_text_Is_abc_hashB_def_AND_delimiters_Contains_BOLD_WHEN_FindToken_Called_For_Third_Time_THEN_Returns_Token_For_Text_def()
        {
            const string thirdTokenText = "def";
            const string text = "abc#b" + thirdTokenText;

            var reader = new WaneTextReader(new string[] { DefaultDelimeterValues.Bold });
            reader.SetText(text);

            reader.ReadNextToken();
            reader.ReadNextToken();
            Token token = reader.ReadNextToken();

            token.Text.ShouldEqual(thirdTokenText);
            token.TokenType.ShouldEqual(TokenType.Text);
            token.LinePosition.ShouldEqual(1);
            token.CharPosition.ShouldEqual(6);
        }

        [TestMethod]
        public void ReadNextToken_GIVEN_Setup_text_Is_abc_hashB_def_AND_delimiters_Contains_BOLD_WHEN_FindToken_Called_For_Third_Time_THEN_Sets_IsEndOfText_To_False()
        {
            const string thirdTokenText = "def";
            const string text = "abc#b" + thirdTokenText;

            var reader = new WaneTextReader(new string[] { DefaultDelimeterValues.Bold });
            reader.SetText(text);

            reader.ReadNextToken();
            reader.ReadNextToken();
            reader.ReadNextToken();

            reader.IsEndOfStream.ShouldBeTrue();
        }

        [TestMethod]
        public void ReadNextToken_GIVEN_Setup_text_Is_hashb_hashi_newline_abc_hashi_hashB_WHEN_Six_Calls_Made_To_FindToken_Correct_Delimiters_And_Text_Tokens_Returned_In_CorrectOrder()
        {
            const string newLine = "\n";
            const string actualText = "abc";

            string text = DefaultDelimeterValues.Bold + DefaultDelimeterValues.Italics + newLine + actualText + DefaultDelimeterValues.Italics + DefaultDelimeterValues.Bold;

            var reader = new WaneTextReader(new string[] { DefaultDelimeterValues.Bold, DefaultDelimeterValues.Italics });
            reader.SetText(text);

            Token firstToken = reader.ReadNextToken();
            Token secondToken = reader.ReadNextToken();
            Token thirdToken = reader.ReadNextToken();
            Token fourthToken = reader.ReadNextToken();
            Token fifthToken = reader.ReadNextToken();
            Token sixthToken = reader.ReadNextToken();

            firstToken.TokenType.ShouldEqual(TokenType.Delimiter, "First Token : Token Type incorrect");
            firstToken.Text.ShouldEqual(DefaultDelimeterValues.Bold, "First Token : text incorrect");

            secondToken.TokenType.ShouldEqual(TokenType.Delimiter, "Second Token : Token Type incorrect");
            secondToken.Text.ShouldEqual(DefaultDelimeterValues.Italics, "Second Token : text incorrect");

            thirdToken.TokenType.ShouldEqual(TokenType.NewLine, "Third Token : Token Type incorrect");
            thirdToken.Text.ShouldEqual(newLine, "Third Token : text incorrect");

            fourthToken.TokenType.ShouldEqual(TokenType.Text, "Fourth Token : Token Type incorrect");
            fourthToken.Text.ShouldEqual(actualText, "Fourth Token : text incorrect");

            fifthToken.TokenType.ShouldEqual(TokenType.Delimiter, "Fifth Token : Token Type incorrect");
            fifthToken.Text.ShouldEqual(DefaultDelimeterValues.Italics, "Fifth Token : text incorrect");

            sixthToken.TokenType.ShouldEqual(TokenType.Delimiter, "Sixth Token : Token Type incorrect");
            sixthToken.Text.ShouldEqual(DefaultDelimeterValues.Bold, "Sixth Token : text incorrect");
        }

        [TestMethod]
        public void Find_GIVEN_text_hashb_WHEN_tokenTextToFind_is_hashB_THEN_Returns_Token_For_hashB()
        {
            const string Text = "ABC";
            string stream = String.Format("{0}{1}", Text, DefaultDelimeterValues.Bold);


            var reader = new WaneTextReader(new string[] { DefaultDelimeterValues.Bold, DefaultDelimeterValues.Italics });
            reader.SetText(stream);

            Token foundToken = reader.Find(DefaultDelimeterValues.Bold);

            foundToken.ShouldNotBeNull("Could not find token");
            foundToken.TokenType.ShouldEqual(TokenType.Delimiter, "Token Type incorrect");
            foundToken.Text.ShouldEqual(DefaultDelimeterValues.Bold, "text incorrect");
        }

        [TestMethod]
        public void Find_GIVEN_text_hashb_WHEN_tokenTextToFind_is_hashI_THEN_Returns_Null()
        {
            const string Text = "ABC";
            string stream = String.Format("{0}{1}", Text, DefaultDelimeterValues.Bold);


            var reader = new WaneTextReader(new string[] { DefaultDelimeterValues.Bold, DefaultDelimeterValues.Italics });
            reader.SetText(stream);

            Token foundToken = reader.Find(DefaultDelimeterValues.Italics);

            foundToken.ShouldBeNull("Should not be able to find italics");
        }

    }
}
