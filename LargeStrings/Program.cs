var bookTitle = ".NET in Action";
Console.WriteLine(@$"<?xml version=""1.0""?>
<books>
  <book title=""{bookTitle}""/>
</books>
");

var userId = 12;
var resourceId = 6;
Console.WriteLine(
@$"{{
  ""RequestParams"": {{
    ""UserId"": ""{userId}""
    ""ResourceId"": ""{resourceId}""
  }}
}}");

var rawJsonString = """
{
  "RequestParams": {
    "UserId": "someId",
    "ResourceId": "someOtherId"
  }
}
""";
Console.WriteLine(rawJsonString);