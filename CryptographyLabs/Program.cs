using CryptographyLabs.DES;

var des = new DES();
var lol = des.Encrypt("Gay", "ijfgjrijrigfregboregregfkreokoglbpglnpgnltgntn");

Console.WriteLine(lol);

Console.WriteLine(des.Decrypt(lol, "ijfgjrijrigfregboregregfkreokoglbpglnpgnltgntn"));