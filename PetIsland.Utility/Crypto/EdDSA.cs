using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Signers;

namespace PetIsland.Utility.Crypto
{
    public static class EdDSA
    {
        public static bool Verify(string base64Data, string base64Signature, string base64PublicKey)
        {
            try
            {
                byte[] publicKeyBytes = Convert.FromBase64String(base64PublicKey);
                byte[] signatureBytes = Convert.FromBase64String(base64Signature);
                byte[] dataBytes = Convert.FromBase64String(base64Data);

                var publicKey = new Ed25519PublicKeyParameters(publicKeyBytes, 0);

                var verifier = new Ed25519Signer();
                verifier.Init(false, publicKey);
                verifier.BlockUpdate(dataBytes, 0, dataBytes.Length);

                return verifier.VerifySignature(signatureBytes);
            }
            catch
            {
                return false;
            }
        }
    }
}
