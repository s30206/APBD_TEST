// using TavernSystem.Models.DTO;
//
// namespace TavernSystem.Application;
//
// public class PersonValidator
// {
//     public static bool ValidateId(AdventurerDTO person)
//     {
//         var id = person.PersonDataId;
//         
//         var letters = id[0].ToString() + id[1].ToString();
//         var chars = letters.ToCharArray();
//         foreach (var c in chars)
//             if (!char.IsAsciiLetterUpper(c))
//                 return false;
//         
//         var numberString = id[2].ToString() + id[3].ToString() + id[4].ToString() + id[5].ToString();
//         var number = int.Parse(numberString);
//         
//         if (number <  || number > 9)
//     }
// }