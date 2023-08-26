using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public static class SystemSafety
{
	private static MD5 _md5Privider = new MD5CryptoServiceProvider();

	private static DESCryptoServiceProvider _desProvider = new DESCryptoServiceProvider();

	public static string StringEncryptByRSA(string encryptInfo, string key = "key")
	{
		using (new RSACryptoServiceProvider(new CspParameters
		{
			KeyContainerName = key
		}))
		{
			return Convert.ToBase64String(BytesEncryptByRSA(Encoding.Default.GetBytes(encryptInfo), key));
		}
	}

	public static string StringDecryptByRSA(string decryptInfo, string key = "key")
	{
		using (new RSACryptoServiceProvider(new CspParameters
		{
			KeyContainerName = key
		}))
		{
			byte[] decryptInfo2 = Convert.FromBase64String(decryptInfo);
			return Encoding.Default.GetString(BytesDecryptByRSA(decryptInfo2, key));
		}
	}

	public static byte[] BytesEncryptByRSA(byte[] encryptInfo, string key = "key")
	{
		using (RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider(new CspParameters
		{
			KeyContainerName = key
		}))
		{
			return rSACryptoServiceProvider.Encrypt(encryptInfo, false);
		}
	}

	public static byte[] BytesDecryptByRSA(byte[] decryptInfo, string key = "key")
	{
		using (RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider(new CspParameters
		{
			KeyContainerName = key
		}))
		{
			return rSACryptoServiceProvider.Decrypt(decryptInfo, false);
		}
	}

	public static byte[] BytesEncryptByMD5(byte[] encryptInfo)
	{
		return _md5Privider.ComputeHash(encryptInfo);
	}

	public static string StringEncryptByMD5(string encryptInfo)
	{
		return BytesEncryptToStringByMD5(Encoding.Default.GetBytes(encryptInfo));
	}

	public static string BytesEncryptToStringByMD5(byte[] encryptInfo)
	{
		string empty = string.Empty;
		try
		{
			return Convert.ToBase64String(BytesEncryptByMD5(encryptInfo));
		}
		catch (Exception ex)
		{
			empty = string.Empty;
			throw new Exception("SystemSafety.BytesEncryptToStringByMD5 ------>\n" + ex.Message);
		}
	}

	public static string PathEncryptToStringByMD5(string path, int bufferSize = 16384)
	{
		string empty = string.Empty;
		if (!File.Exists(path))
		{
			return empty;
		}
		try
		{
			byte[] array = new byte[bufferSize];
			using (Stream stream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read))
			{
				int num = 0;
				byte[] outputBuffer = new byte[bufferSize];
				while ((num = stream.Read(array, 0, array.Length)) > 0)
				{
					_md5Privider.TransformBlock(array, 0, num, outputBuffer, 0);
				}
				_md5Privider.TransformFinalBlock(array, 0, 0);
				empty = Convert.ToBase64String(_md5Privider.Hash);
				_md5Privider.Clear();
				return empty;
			}
		}
		catch (Exception ex)
		{
			empty = string.Empty;
			throw new Exception("SystemSafety.PathEncryptToStringByMD5 ------>\n" + ex.Message);
		}
	}

	public static string StringEncryptByDES(string encryptInfo, string key = "20150528", string iv = "12345678")
	{
		try
		{
			byte[] bytes = Encoding.UTF8.GetBytes(key);
			byte[] bytes2 = Encoding.UTF8.GetBytes(iv);
			using (MemoryStream memoryStream = new MemoryStream())
			{
				byte[] bytes3 = Encoding.UTF8.GetBytes(encryptInfo);
				try
				{
					using (CryptoStream cryptoStream = new CryptoStream(memoryStream, _desProvider.CreateEncryptor(bytes, bytes2), CryptoStreamMode.Write))
					{
						cryptoStream.Write(bytes3, 0, bytes3.Length);
						cryptoStream.FlushFinalBlock();
					}
					return Convert.ToBase64String(memoryStream.ToArray());
				}
				catch
				{
					Debug.Log("String Encrypt By DES is error!!!! : " + encryptInfo);
					return encryptInfo;
				}
			}
		}
		catch
		{
			Debug.Log("String Encrypt By DES is error!!!! : " + encryptInfo);
			return "";
		}
	}

	public static string StringDecryptByDES(string encryptedString, string key = "20150528", string iv = "12345678")
	{
		byte[] bytes = Encoding.UTF8.GetBytes(key);
		byte[] bytes2 = Encoding.UTF8.GetBytes(iv);
		using (MemoryStream memoryStream = new MemoryStream())
		{
			try
			{
				byte[] array = Convert.FromBase64String(encryptedString);
				using (CryptoStream cryptoStream = new CryptoStream(memoryStream, _desProvider.CreateDecryptor(bytes, bytes2), CryptoStreamMode.Write))
				{
					cryptoStream.Write(array, 0, array.Length);
					cryptoStream.FlushFinalBlock();
				}
				return Encoding.UTF8.GetString(memoryStream.ToArray());
			}
			catch
			{
				Debug.Log("String Decrypt By DES is error!!!! : " + encryptedString);
				return "";
			}
		}
	}
}
