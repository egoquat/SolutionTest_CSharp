using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.IO.Compression;
using System.Runtime.InteropServices;

namespace _190715_GZipStreamTest
{
    class Program
    {
        //static Int32 lengthMassive = Int32.MaxValue - 1;
        //static long lengthMassive = (long)(Int32.MaxValue - 1);
        static long lengthMassive = 268435455;

        static bool ConvertBufferPack(byte[] buffer, ref long[] buffers_out, ref long lengthBuffer)
        {
            lengthBuffer = buffer.Length;
            if (buffer.Length <= 0)
            {
                return false;
            }

            List<long> BuffersTemp = new List<long>();

            int iter = 0;
            for (int i = 0; i < buffer.Length; i = i + 8)
            {
                int idxStart = i;
                int remainBuffer = buffer.Length - idxStart;
                long packed = 0;
                if (remainBuffer < 8)
                {
                    byte[] bufferTemp = new byte[8];
                    for(int idx = 0; idx < remainBuffer; ++idx)
                    {
                        bufferTemp[idx] = buffer[idxStart + idx];
                    }
                    packed = BitConverter.ToInt64( bufferTemp, 0 );
                }
                else
                {
                    packed = BitConverter.ToInt64( buffer, idxStart );
                }
                BuffersTemp.Add(packed);
                ++iter;
            }

            buffers_out = BuffersTemp.ToArray();
            return buffers_out.Length >= 1;
        }

        static bool GetDecompressGZip(GZipStream gzipStream, ref long[] buffers_out, ref long lengthBuffer_out)
        {
            byte[] buffer = null;
            try
            {
                buffer = new byte[1024 * 1024 * 512];
            }
            catch(Exception e)
            {
                Console.WriteLine("GetDecompressGZip:buffer:/Exception:" + e);
                return false;
            }

            int count;
            long offset = 0;
            List<long> BuffersTemp = new List<long>();
            int countBufferAdded = 0;

            try
            {
                while (true)
                {
                    count = gzipStream.Read(buffer, 0, buffer.Length);
                    if (count <= 0)
                    {
                        break;
                    }

                    int iter = 0;
                    for (int i = 0; i < count; i = i + 8)
                    {
                        int idxStart = (iter * 8);
                        long packed = BitConverter.ToInt64( buffer, idxStart );
                        BuffersTemp.Add(packed);
                        ++iter;
                        ++countBufferAdded;
                    }
                
                    offset = offset + count;
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("GetDecompressGZip:countBufferAdded("+countBufferAdded+"):/Exception:" + e);
                return false;
            }

            buffers_out = BuffersTemp.ToArray();
            lengthBuffer_out = offset;
            return lengthBuffer_out >= 1;
        }

        static Int32 ReadByte(byte[] buffer, Int32 position, Int32 sizeRead, ref byte[] buffer_out)
        {
            Int32 DataRemaining = buffer.Length - position;
		    if (DataRemaining <= 0)
            {
                return 0;
            }

            Int32 sizeReadUse = DataRemaining < sizeRead ? DataRemaining : sizeRead;
            //DataOut = Marshal.AllocHGlobal(sizeReadUse);
            //Marshal.Copy(buffer, position, DataOut, sizeReadUse);

            buffer_out = new byte[sizeReadUse];
            Array.Copy(buffer, position, buffer_out, 0, sizeReadUse);

            return sizeReadUse;
        }

        static Int32 ReadByte(byte[] buffer, Int32 position, Int32 sizeRead, IntPtr dataOut)
        {
            byte[] bufferRead = null;
            Int32 readByte = ReadByte(buffer, position, sizeRead, ref bufferRead);
            Marshal.Copy(bufferRead, 0, dataOut, bufferRead.Length);
            return readByte;
        }

        static Int32 ReadBytePack(long[] buffer, long position, Int32 sizeRead, long lengthBuffer, ref byte[] buffer_out)
        {
            Int32 sizeReadUse = sizeRead;
            if (position + sizeReadUse > lengthBuffer)
            {
                sizeReadUse = (Int32)(lengthBuffer - position);
            }

            Int32 positionStartLong = (Int32)(position / 8);
            Int32 sizeReadLong = sizeReadUse / 8;

            Int32 positionRemain = (Int32)(position % 8);
            Int32 sizeReadRemain = sizeReadUse % 8;

            Int32 sizeReadLongUse = sizeReadLong + (sizeReadRemain >= 1 ? 1 : 0) + (positionRemain >= 1 ? 1 : 0);
            if( sizeReadLongUse <= 0 )
                return 0;

            Int32 remaining = buffer.Length - positionStartLong;
            if( remaining <= 0 )
                return 0;

            Int32 sizeReadByteFirstRead = sizeReadLongUse * 8;
            sizeReadLongUse = remaining < sizeReadLongUse ? remaining : sizeReadLongUse;

            byte[] bufferBytesFirstRead = new byte[sizeReadByteFirstRead];
            for( int i = positionStartLong; i < positionStartLong + sizeReadLongUse; ++i )
            {
                byte[] byteUnpacked = BitConverter.GetBytes(buffer[i]);
                int idx = (i - positionStartLong) * 8;
                if (idx > sizeReadByteFirstRead)
                {
                    break;
                }

                int remainingByte = sizeReadByteFirstRead - idx;
                int readByte = 8 > remainingByte ? remainingByte : 8;
                if (readByte <= 0)
                {
                    break;
                }

                Array.Copy(byteUnpacked, 0, bufferBytesFirstRead, idx , readByte);
            }
                        
            buffer_out = new byte[sizeReadUse];
            Array.Copy(bufferBytesFirstRead, positionRemain, buffer_out, 0, sizeReadUse);

            return sizeReadUse;
        }

        static Int32 ReadBytePack(long[] buffer, long position, Int32 sizeRead, long lengthBuffer, IntPtr dataOut)
        {
            byte[] bufferRead = null;
            Int32 readByte = ReadBytePack(buffer, position, sizeRead, lengthBuffer, ref bufferRead);
            Marshal.Copy(bufferRead, 0, dataOut, bufferRead.Length);
            return readByte;
        }

        static bool CompareBuffersTest(byte[] bufferByte, long[] bufferLong, long lengthBuffer)
        {
            int CountTest = 2000;
            Random rand = new Random();
            for (int i = 0; i < CountTest; ++i)
            {
                long position = rand.Next() % bufferByte.Length;
                int readLineIter = 16 + (rand.Next() % 655412);
                int readActual = 0, readActualUnpack = 0;

                //IntPtr dataInt32 = new IntPtr(), dataLong = new IntPtr();
                //
                //ReadByte(bufferByte, position, readLineIter, ref dataInt32);
                //ReadBytePack(ChannelDataPacked, position, readLineIter, ref dataLong);

                byte[] dataInt32 = null, dataLong = null;

                readActual = ReadByte(bufferByte, (int)position, readLineIter, ref dataInt32);
                readActualUnpack = ReadBytePack(bufferLong, position, readLineIter, lengthBuffer, ref dataLong);

                bool isValidated = true;
                if (null == dataInt32 || null == dataLong)
                {
                    isValidated = false;
                    Console.WriteLine("Error.(null == dataInt32 || null == dataLong)");
                }
                else if (dataInt32.Length != dataLong.Length)
                {
                    isValidated = false;
                    Console.WriteLine("Error.(dataInt32.Length != dataLong.Length)");
                }
                else
                {
                    for(int idx = 0; idx < dataInt32.Length; ++idx)
                    {
                        if (dataInt32[idx] != dataLong[idx])
                        {
                            isValidated = false;
                            Console.WriteLine("************************** Error.");
                        }
                    }
                }

                Console.WriteLine("ReadByte/isValidated("+isValidated+")/readLineIter:"+readLineIter+"/readActual:" + readActual + "/readActualUnpack:" + readActualUnpack);
            }

            return true;
        }

        static void MainGZip()
        {
            //string FullNameFile = "D:\\Test\\C#\\SolutionTest_CSharp\\Data\\GZipTest\\v1.8DFE99084080D73718701FBAACA3376E";   //2gb
            //string FullManagedName = "D:\\Test\\C#\\SolutionTest_CSharp\\Data\\GZipTest\\v1.8DFE99084080D73718701FBAACA3376E.zip";   //2gb gzip -1
            string FullManagedName = "D:\\Test\\C#\\SolutionTest_CSharp\\Data\\GZipTest\\v1.5871438041EAA8C4FE0EA3AA99EB227C.scenegz";   //2gb gzip -2
            //string FullManagedName = "D:\\Test\\C#\\SolutionTest_CSharp\\Data\\GZipTest\\v1.57582A9441A33ECCA4C319BDDAFEE81A.zip"; //1gb under.

            //if (File.Exists(FullNameFile))
            //{
            //    //byte[] bytes = File.ReadAllBytes(FullNameFile);
            //    //long testnumber = 1 << 62;
            //    //byte[] bytes = new byte[testnumber];
            //    Stream stream = File.OpenRead(FullNameFile);
            //    Console.WriteLine("[Interface:OpenChannel] : stream:" + stream.Length);
            //    
            //}

            if (File.Exists(FullManagedName))
			{
                Console.WriteLine("[Interface:OpenChannel] : Uncompressed Start File:" + FullManagedName);

				byte[] RawCompressedData = File.ReadAllBytes(FullManagedName);

                Console.WriteLine("[Interface:OpenChannel] : Read Channel File Done. byte:" + RawCompressedData.Length);

				// http://www.ietf.org/rfc/rfc1952.txt?number=1952
				Int32 UncompressedSize = BitConverter.ToInt32(RawCompressedData, RawCompressedData.Length - 4);
                
                Console.WriteLine("[Interface:OpenChannel] : file UncompressedSize:" + UncompressedSize);

				//NewChannelInfo.ChannelData = new byte[UncompressedSize];

                //BLC_EDIT_START : Overflow compressed 32 to 64.
                // Open the decompression stream and decompress directly into the destination
                GZipStream gzipStream = null; 
                long[] ChannelDataPacked = null;
                long lengthBuffer = 0;
                byte[] ChannelData = null;
                if (UncompressedSize <= -1)
                {
                    gzipStream = new GZipStream(new MemoryStream(RawCompressedData), CompressionMode.Decompress, false);
                    GetDecompressGZip(gzipStream, ref ChannelDataPacked, ref lengthBuffer);
                    gzipStream.Close();
                }
                else
                {
                    gzipStream = new GZipStream(new MemoryStream(RawCompressedData), CompressionMode.Decompress, false);
                    ChannelData = new byte[UncompressedSize];
                    Int32 UncompressedSize32 = (Int32)UncompressedSize;
					gzipStream.Read(ChannelData, 0, UncompressedSize32);
					gzipStream.Close();
                }

                //ConvertBufferPack(ChannelData, ref ChannelDataPacked, ref lengthBuffer);

                //CompareBuffersTest(ChannelData, ChannelDataPacked, lengthBuffer);

                Console.WriteLine("[Interface:OpenChannel] : Uncompressed Done.");
			}
			else
			{
				// Failed to find the channel to read, return an error
				Console.WriteLine("[Interface:OpenChannel] : Uncompressed Error.");
			}

        }

        static long[] MainArrayLimit()
        {
            long[] BuffersTemp = new long[lengthMassive];
            
            for (long i = 0; i < lengthMassive; ++i)
	        {
		        BuffersTemp[i] = (byte)(i % 65535);
                if (i % 11433 == 0)
                {
                    Console.WriteLine("BuffersTemp[%d]=%d", i, BuffersTemp[i]);
                }
	        }

            return BuffersTemp;
        }

        static void Main(string[] args)
        {
            //MainArrayLimit();
            MainGZip();
        }
    }
}
