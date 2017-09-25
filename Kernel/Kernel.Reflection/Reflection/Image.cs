namespace Kernel.Reflection
{
	using System;
	using System.IO;

	/// <summary>
	/// Build based on reference http://stackoverflow.com/questions/270531/how-to-determine-if-a-net-assembly-was-built-for-x86-or-x64 and others regarding it
	/// </summary>
	class Image : IDisposable
	{
		private readonly Stream Stream;

		public static CompilationMode GetCompilationMode(string file)
		{
			if (String.IsNullOrWhiteSpace(file))
			{
				throw new ArgumentNullException("file", "You must specify a file name");
			}

			var stream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read);

			using (var image = new Image(stream))
			{
				return image.GetCompilationMode();
			}
		}

		private Image(Stream stream)
		{
			this.Stream = stream;
		}

		private CompilationMode GetCompilationMode()
		{
			if (Stream.Length < 318)
				return CompilationMode.NativeOrInvalid;

			if (ReadUInt16() != 0x5a4d)
				return CompilationMode.NativeOrInvalid;

			if (!Advance(58))
				return CompilationMode.NativeOrInvalid;

			if (!MoveTo(ReadUInt32()))
				return CompilationMode.NativeOrInvalid;

			if (ReadUInt32() != 0x00004550)
				return CompilationMode.NativeOrInvalid;

			if (!Advance(20))
				return CompilationMode.NativeOrInvalid;

			var result = CompilationMode.NativeOrInvalid;

			switch (ReadUInt16())
			{
				case 0x10B:
					if (Advance(206))
					{
						result = CompilationMode.CLRx86;
					}

					break;
				case 0x20B:
					if (Advance(222))
					{
						result = CompilationMode.CLRx64;
					}
					break;
			}

			if (result == CompilationMode.NativeOrInvalid)
			{
				return result;
			}

			return ReadUInt32() != 0 ? result : CompilationMode.NativeOrInvalid;
		}

		private bool Advance(int length)
		{
			if (Stream.Position + length >= Stream.Length)
				return false;

			Stream.Seek(length, SeekOrigin.Current);
			return true;
		}

		private bool MoveTo(uint position)
		{
			if (position >= Stream.Length)
				return false;

			Stream.Position = position;

			return true;
		}

		private ushort ReadUInt16()
		{
			return (ushort)(Stream.ReadByte()
							| (Stream.ReadByte() << 8));
		}

		private uint ReadUInt32()
		{
			return (uint)(Stream.ReadByte()
						  | (Stream.ReadByte() << 8)
						  | (Stream.ReadByte() << 16)
						  | (Stream.ReadByte() << 24));
		}

		void IDisposable.Dispose()
		{
			Stream.Dispose();
		}
	}
}
