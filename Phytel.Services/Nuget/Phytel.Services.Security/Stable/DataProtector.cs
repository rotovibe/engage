using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace Phytel.Services.Security
{
	/// <summary>
	/// Summary description for DataProtection.
	/// </summary>
	public class DataProtector
	{
		[DllImport("Crypt32.dll", SetLastError = true, CharSet = System.Runtime.InteropServices.CharSet.Unicode)]
		private static extern bool CryptProtectData(ref DATA_BLOB pDataIn, String szDataDescr, ref DATA_BLOB pOptionalEntropy, IntPtr pvReserved, ref CRYPTPROTECT_PROMPTSTRUCT pPromptStruct, int dwFlags, ref DATA_BLOB pDataOut);

        [DllImport("Crypt32.dll", SetLastError = true, CharSet = System.Runtime.InteropServices.CharSet.Unicode)]
		private static extern bool CryptUnprotectData(ref DATA_BLOB pDataIn, String szDataDescr, ref DATA_BLOB pOptionalEntropy, IntPtr pvReserved, ref CRYPTPROTECT_PROMPTSTRUCT pPromptStruct, int dwFlags, ref DATA_BLOB pDataOut);

		[DllImport("kernel32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
		private unsafe static extern int FormatMessage(int dwFlags, ref IntPtr lpSource, int dwMessageId, int dwLanguageId, ref String lpBuffer, int nSize, IntPtr* Arguments);

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct DATA_BLOB : IDisposable
		{
			public int cbData;
			public IntPtr pbData;

            public void Dispose()
            {
                
            }
        }

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct CRYPTPROTECT_PROMPTSTRUCT
		{
			public int cbSize;
			public int dwPromptFlags;
			public IntPtr hwndApp;
			public String szPrompt;
		}
		static private IntPtr NullPtr = ((IntPtr)((int)(0)));
		private const int CRYPTPROTECT_UI_FORBIDDEN = 0x1;
		private const int CRYPTPROTECT_LOCAL_MACHINE = 0x4;
		private const string _simplePassPhrase = "Phytel@letyhP";
		private const string _simpleSaltValue = "&*JDUHYEOK";
		private const string _simpleHash = "SHA1";
		private const int _simplePwdIterations = 3;
		private const string _simpleVector = "@1C2c3x4e5Y6g7H7";
		private const int _simpleKeySize = 256;

		public enum Store { USE_MACHINE_STORE = 1, USE_USER_STORE, USE_SIMPLE_STORE };
		private Store store;

		public enum EntropyType { KEYDATA, CONFIGPATH };

		public DataProtector(Store tempStore)
		{
			store = tempStore;
		}

		public static string GetConfigurationPath()
		{
			// Get the global configuration directory for the application settings 
            return @"C:\Program Files\Phytel.Net\config-prod\";
		}

		public static string GetConfigurationPathAndFile()
		{
			return DataProtector.GetConfigurationPath() + "PhytelServices.config";
		}
		
		private byte[] Encrypt(byte[] plainText, byte[] optionalEntropy)
		{
			bool retVal = false;
			DATA_BLOB plainTextBlob = new DATA_BLOB();
			DATA_BLOB cipherTextBlob = new DATA_BLOB();
			DATA_BLOB entropyBlob = new DATA_BLOB();
			CRYPTPROTECT_PROMPTSTRUCT prompt = new CRYPTPROTECT_PROMPTSTRUCT();
			InitPromptstruct(ref prompt);
			int dwFlags;
			try
			{
				try
				{
					int bytesSize = plainText.Length;
					plainTextBlob.pbData = Marshal.AllocHGlobal(bytesSize);
					if (IntPtr.Zero == plainTextBlob.pbData)
					{
						throw new Exception("Unable to allocate plaintext buffer.");
					}
					plainTextBlob.cbData = bytesSize;
					Marshal.Copy(plainText, 0, plainTextBlob.pbData, bytesSize);
				}
				catch (Exception ex)
				{
					throw new Exception("Exception marshalling data. " + ex.Message);
				}
				if (Store.USE_MACHINE_STORE == store)
				{//Using the machine store, should be providing entropy.
					dwFlags = CRYPTPROTECT_LOCAL_MACHINE | CRYPTPROTECT_UI_FORBIDDEN;
					//Check to see if the entropy is null
					if (null == optionalEntropy)
					{//Allocate something
						optionalEntropy = new byte[0];
					}
					try
					{
						int bytesSize = optionalEntropy.Length;
						entropyBlob.pbData = Marshal.AllocHGlobal(optionalEntropy.Length); ;
						if (IntPtr.Zero == entropyBlob.pbData)
						{
							throw new Exception("Unable to allocate entropy data buffer.");
						}
						Marshal.Copy(optionalEntropy, 0, entropyBlob.pbData, bytesSize);
						entropyBlob.cbData = bytesSize;
					}
					catch (Exception ex)
					{
						throw new Exception("Exception entropy marshalling data. " +
							ex.Message);
					}
				}
				else
				{//Using the user store
					dwFlags = CRYPTPROTECT_UI_FORBIDDEN;
				}
				retVal = CryptProtectData(ref plainTextBlob, "", ref entropyBlob, IntPtr.Zero, ref prompt, dwFlags, ref cipherTextBlob);
				if (false == retVal)
				{
					throw new Exception("Encryption failed. " +
						GetErrorMessage(Marshal.GetLastWin32Error()));
				}
				//Free the blob and entropy.
				if (IntPtr.Zero != plainTextBlob.pbData)
				{
					Marshal.FreeHGlobal(plainTextBlob.pbData);
				}
				if (IntPtr.Zero != entropyBlob.pbData)
				{
					Marshal.FreeHGlobal(entropyBlob.pbData);
				}
			}
			catch (Exception ex)
			{
				throw new Exception("Exception encrypting. " + ex.Message);
			}
			byte[] cipherText = new byte[cipherTextBlob.cbData];
			Marshal.Copy(cipherTextBlob.pbData, cipherText, 0, cipherTextBlob.cbData);
			Marshal.FreeHGlobal(cipherTextBlob.pbData);
			return cipherText;
		}

		private byte[] Decrypt(byte[] cipherText, byte[] optionalEntropy)
		{
			bool retVal = false;
			DATA_BLOB plainTextBlob = new DATA_BLOB();
			DATA_BLOB cipherBlob = new DATA_BLOB();
			CRYPTPROTECT_PROMPTSTRUCT prompt = new
				CRYPTPROTECT_PROMPTSTRUCT();
			InitPromptstruct(ref prompt);
			try
			{
				try
				{
					int cipherTextSize = cipherText.Length;
					cipherBlob.pbData = Marshal.AllocHGlobal(cipherTextSize);
					if (IntPtr.Zero == cipherBlob.pbData)
					{
						throw new Exception("Unable to allocate cipherText buffer.");
					}
					cipherBlob.cbData = cipherTextSize;
					Marshal.Copy(cipherText, 0, cipherBlob.pbData,
						cipherBlob.cbData);
				}
				catch (Exception ex)
				{
					throw new Exception("Exception marshalling data. " +
						ex.Message);
				}
				DATA_BLOB entropyBlob = new DATA_BLOB();
				int dwFlags;
				if (Store.USE_MACHINE_STORE == store)
				{//Using the machine store, should be providing entropy.
					dwFlags =
						CRYPTPROTECT_LOCAL_MACHINE | CRYPTPROTECT_UI_FORBIDDEN;
					//Check to see if the entropy is null
					if (null == optionalEntropy)
					{//Allocate something
						optionalEntropy = new byte[0];
					}
					try
					{
						int bytesSize = optionalEntropy.Length;
						entropyBlob.pbData = Marshal.AllocHGlobal(bytesSize);
						if (IntPtr.Zero == entropyBlob.pbData)
						{
							throw new Exception("Unable to allocate entropy buffer.");
						}
						entropyBlob.cbData = bytesSize;
						Marshal.Copy(optionalEntropy, 0, entropyBlob.pbData,
							bytesSize);
					}
					catch (Exception ex)
					{
						throw new Exception("Exception entropy marshalling data. " +
							ex.Message);
					}
				}
				else
				{//Using the user store
					dwFlags = CRYPTPROTECT_UI_FORBIDDEN;
				}
				retVal = CryptUnprotectData(ref cipherBlob, null, ref 
					entropyBlob,
					IntPtr.Zero, ref prompt, dwFlags,
					ref plainTextBlob);
				if (false == retVal)
				{
					throw new Exception("Decryption failed. " +
						GetErrorMessage(Marshal.GetLastWin32Error()));
				}
				//Free the blob and entropy.
				if (IntPtr.Zero != cipherBlob.pbData)
				{
					Marshal.FreeHGlobal(cipherBlob.pbData);
				}
				if (IntPtr.Zero != entropyBlob.pbData)
				{
					Marshal.FreeHGlobal(entropyBlob.pbData);
				}
			}
			catch (Exception ex)
			{
				throw new Exception("Exception decrypting. " + ex.Message);
			}
			byte[] plainText = new byte[plainTextBlob.cbData];
			Marshal.Copy(plainTextBlob.pbData, plainText, 0, plainTextBlob.cbData);
			Marshal.FreeHGlobal(plainTextBlob.pbData);
			return plainText;
		}

		private void InitPromptstruct(ref CRYPTPROTECT_PROMPTSTRUCT ps)
		{
			ps.cbSize = Marshal.SizeOf(typeof(CRYPTPROTECT_PROMPTSTRUCT));
			ps.dwPromptFlags = 0;
			ps.hwndApp = NullPtr;
			ps.szPrompt = null;
		}

		private unsafe static String GetErrorMessage(int errorCode)
		{
			int FORMAT_MESSAGE_ALLOCATE_BUFFER = 0x00000100;
			int FORMAT_MESSAGE_IGNORE_INSERTS = 0x00000200;
			int FORMAT_MESSAGE_FROM_SYSTEM = 0x00001000;
			int messageSize = 255;
			String lpMsgBuf = "";
			int dwFlags = FORMAT_MESSAGE_ALLOCATE_BUFFER |
				FORMAT_MESSAGE_FROM_SYSTEM |
				FORMAT_MESSAGE_IGNORE_INSERTS;
			IntPtr ptrlpSource = new IntPtr();
			IntPtr prtArguments = new IntPtr();
			int retVal = FormatMessage(dwFlags, ref ptrlpSource, errorCode, 0,
				ref lpMsgBuf, messageSize,
				&prtArguments);
			if (0 == retVal)
			{
				throw new Exception("Failed to format message for error code " +
					errorCode + ". ");
			}
			return lpMsgBuf;
		}

		public string Encrypt(string encryptString)
		{
			byte[] entropy;
			string returnEncryptedString = string.Empty;

			//First, check the store.  If the Store is USE_SIMPLE_STORE, we need to use the SimpleDataProtector Class
			if (store == Store.USE_SIMPLE_STORE)
				returnEncryptedString = SimpleDataProtector.Encrypt(encryptString, _simplePassPhrase, _simpleSaltValue, _simpleHash, _simplePwdIterations, _simpleVector, _simpleKeySize);
			else
			{
				entropy = Encoding.ASCII.GetBytes(GetEntropyKey());

				byte[] data = Encoding.ASCII.GetBytes(encryptString);
				returnEncryptedString = Convert.ToBase64String(this.Encrypt(data, entropy));
			}
			return returnEncryptedString;
		}

		public string Decrypt(string encryptString)
		{
			byte[] entropy;
			string returnDecryptedString = string.Empty;

			//First, check the store.  If the Store is USE_SIMPLE_STORE, we need to use the SimpleDataProtector Class
			if (store == Store.USE_SIMPLE_STORE)
				returnDecryptedString = SimpleDataProtector.Decrypt(encryptString, _simplePassPhrase, _simpleSaltValue, _simpleHash, _simplePwdIterations, _simpleVector, _simpleKeySize);
			else
			{
				entropy = Encoding.ASCII.GetBytes(GetEntropyKey());

				byte[] data = Convert.FromBase64String(encryptString);
				returnDecryptedString = Encoding.ASCII.GetString(this.Decrypt(data, entropy));
			}
			return returnDecryptedString;
		}

		private string GetEntropyKey()
		{
			string entropy = "";

			try
			{
				XmlDocument doc = new XmlDocument();
				doc.Load(DataProtector.GetConfigurationPath() + "server.config");

				XmlNode node = doc.SelectSingleNode("//configuration/entropy");

				if ((node != null))
				{
					entropy = node.InnerText;
				}
			}
			catch (System.Exception)
			{
				throw;
			}

			return entropy;
		}
	}

	internal class SimpleDataProtector
	{
		/// <summary>
		/// Encrypts specified plaintext using Rijndael symmetric key algorithm
		/// and returns a base64-encoded result.
		/// </summary>
		/// <param name="plainText">
		/// Plaintext value to be encrypted.
		/// </param>
		/// <param name="passPhrase">
		/// Passphrase from which a pseudo-random password will be derived. The
		/// derived password will be used to generate the encryption key.
		/// Passphrase can be any string. In this example we assume that this
		/// passphrase is an ASCII string.
		/// </param>
		/// <param name="saltValue">
		/// Salt value used along with passphrase to generate password. Salt can
		/// be any string. In this example we assume that salt is an ASCII string.
		/// </param>
		/// <param name="hashAlgorithm">
		/// Hash algorithm used to generate password. Allowed values are: "MD5" and
		/// "SHA1". SHA1 hashes are a bit slower, but more secure than MD5 hashes.
		/// </param>
		/// <param name="passwordIterations">
		/// Number of iterations used to generate password. One or two iterations
		/// should be enough.
		/// </param>
		/// <param name="initVector">
		/// Initialization vector (or IV). This value is required to encrypt the
		/// first block of plaintext data. For RijndaelManaged class IV must be 
		/// exactly 16 ASCII characters long.
		/// </param>
		/// <param name="keySize">
		/// Size of encryption key in bits. Allowed values are: 128, 192, and 256. 
		/// Longer keys are more secure than shorter keys.
		/// </param>
		/// <returns>
		/// Encrypted value formatted as a base64-encoded string.
		/// </returns>
		public static string Encrypt(string plainText,
									 string passPhrase,
									 string saltValue,
									 string hashAlgorithm,
									 int passwordIterations,
									 string initVector,
									 int keySize)
		{
			// Convert strings into byte arrays.
			// Let us assume that strings only contain ASCII codes.
			// If strings include Unicode characters, use Unicode, UTF7, or UTF8 
			// encoding.
			byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
			byte[] saltValueBytes = Encoding.ASCII.GetBytes(saltValue);

			// Convert our plaintext into a byte array.
			// Let us assume that plaintext contains UTF8-encoded characters.
			byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

			// First, we must create a password, from which the key will be derived.
			// This password will be generated from the specified passphrase and 
			// salt value. The password will be created using the specified hash 
			// algorithm. Password creation can be done in several iterations.
			PasswordDeriveBytes password = new PasswordDeriveBytes(
															passPhrase,
															saltValueBytes,
															hashAlgorithm,
															passwordIterations);

			// Use the password to generate pseudo-random bytes for the encryption
			// key. Specify the size of the key in bytes (instead of bits).
			byte[] keyBytes = password.GetBytes(keySize / 8);

			// Create uninitialized Rijndael encryption object.
			RijndaelManaged symmetricKey = new RijndaelManaged();

			// It is reasonable to set encryption mode to Cipher Block Chaining
			// (CBC). Use default options for other symmetric key parameters.
			symmetricKey.Mode = CipherMode.CBC;

			// Generate encryptor from the existing key bytes and initialization 
			// vector. Key size will be defined based on the number of the key 
			// bytes.
			ICryptoTransform encryptor = symmetricKey.CreateEncryptor(
															 keyBytes,
															 initVectorBytes);

			// Define memory stream which will be used to hold encrypted data.
			MemoryStream memoryStream = new MemoryStream();

			// Define cryptographic stream (always use Write mode for encryption).
			CryptoStream cryptoStream = new CryptoStream(memoryStream,
														 encryptor,
														 CryptoStreamMode.Write);
			// Start encrypting.
			cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);

			// Finish encrypting.
			cryptoStream.FlushFinalBlock();

			// Convert our encrypted data from a memory stream into a byte array.
			byte[] cipherTextBytes = memoryStream.ToArray();

			// Close both streams.
			memoryStream.Close();
			cryptoStream.Close();

			// Convert encrypted data into a base64-encoded string.
			string cipherText = Convert.ToBase64String(cipherTextBytes);

			// Return encrypted string.
			return cipherText;
		}

		/// <summary>
		/// Decrypts specified ciphertext using Rijndael symmetric key algorithm.
		/// </summary>
		/// <param name="cipherText">
		/// Base64-formatted ciphertext value.
		/// </param>
		/// <param name="passPhrase">
		/// Passphrase from which a pseudo-random password will be derived. The
		/// derived password will be used to generate the encryption key.
		/// Passphrase can be any string. In this example we assume that this
		/// passphrase is an ASCII string.
		/// </param>
		/// <param name="saltValue">
		/// Salt value used along with passphrase to generate password. Salt can
		/// be any string. In this example we assume that salt is an ASCII string.
		/// </param>
		/// <param name="hashAlgorithm">
		/// Hash algorithm used to generate password. Allowed values are: "MD5" and
		/// "SHA1". SHA1 hashes are a bit slower, but more secure than MD5 hashes.
		/// </param>
		/// <param name="passwordIterations">
		/// Number of iterations used to generate password. One or two iterations
		/// should be enough.
		/// </param>
		/// <param name="initVector">
		/// Initialization vector (or IV). This value is required to encrypt the
		/// first block of plaintext data. For RijndaelManaged class IV must be
		/// exactly 16 ASCII characters long.
		/// </param>
		/// <param name="keySize">
		/// Size of encryption key in bits. Allowed values are: 128, 192, and 256.
		/// Longer keys are more secure than shorter keys.
		/// </param>
		/// <returns>
		/// Decrypted string value.
		/// </returns>
		/// <remarks>
		/// Most of the logic in this function is similar to the Encrypt
		/// logic. In order for decryption to work, all parameters of this function
		/// - except cipherText value - must match the corresponding parameters of
		/// the Encrypt function which was called to generate the
		/// ciphertext.
		/// </remarks>
		public static string Decrypt(string cipherText,
									 string passPhrase,
									 string saltValue,
									 string hashAlgorithm,
									 int passwordIterations,
									 string initVector,
									 int keySize)
		{
			// Convert strings defining encryption key characteristics into byte
			// arrays. Let us assume that strings only contain ASCII codes.
			// If strings include Unicode characters, use Unicode, UTF7, or UTF8
			// encoding.
			byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
			byte[] saltValueBytes = Encoding.ASCII.GetBytes(saltValue);

			// Convert our ciphertext into a byte array.
			byte[] cipherTextBytes = Convert.FromBase64String(cipherText);

			// First, we must create a password, from which the key will be 
			// derived. This password will be generated from the specified 
			// passphrase and salt value. The password will be created using
			// the specified hash algorithm. Password creation can be done in
			// several iterations.
			PasswordDeriveBytes password = new PasswordDeriveBytes(
															passPhrase,
															saltValueBytes,
															hashAlgorithm,
															passwordIterations);

			// Use the password to generate pseudo-random bytes for the encryption
			// key. Specify the size of the key in bytes (instead of bits).
			byte[] keyBytes = password.GetBytes(keySize / 8);

			// Create uninitialized Rijndael encryption object.
			RijndaelManaged symmetricKey = new RijndaelManaged();

			// It is reasonable to set encryption mode to Cipher Block Chaining
			// (CBC). Use default options for other symmetric key parameters.
			symmetricKey.Mode = CipherMode.CBC;

			// Generate decryptor from the existing key bytes and initialization 
			// vector. Key size will be defined based on the number of the key 
			// bytes.
			ICryptoTransform decryptor = symmetricKey.CreateDecryptor(
															 keyBytes,
															 initVectorBytes);

			// Define memory stream which will be used to hold encrypted data.
			MemoryStream memoryStream = new MemoryStream(cipherTextBytes);

			// Define cryptographic stream (always use Read mode for encryption).
			CryptoStream cryptoStream = new CryptoStream(memoryStream,
														  decryptor,
														  CryptoStreamMode.Read);

			// Since at this point we don't know what the size of decrypted data
			// will be, allocate the buffer long enough to hold ciphertext;
			// plaintext is never longer than ciphertext.
			byte[] plainTextBytes = new byte[cipherTextBytes.Length];

			// Start decrypting.
			int decryptedByteCount = cryptoStream.Read(plainTextBytes,
													   0,
													   plainTextBytes.Length);

			// Close both streams.
			memoryStream.Close();
			cryptoStream.Close();

			// Convert decrypted data into a string. 
			// Let us assume that the original plaintext string was UTF8-encoded.
			string plainText = Encoding.UTF8.GetString(plainTextBytes,
													   0,
													   decryptedByteCount);

			// Return decrypted string.   
			return plainText;
		}
	}
}