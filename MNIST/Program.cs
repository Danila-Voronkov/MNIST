using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace MNIST
{
  class ConsoleApplication
  {
  	const string fileName = "heft.dat";
  	public static void WriteDefaultValues(double[,] heft0_1, double[,] heft1_2, double[,] heft2_3, double AllSum, double LearningRate)
  	{
  		File.Delete(fileName);
  		using FileStream stream = File.Open(fileName, FileMode.Create);
  		using BinaryWriter writer = new BinaryWriter(stream, Encoding.UTF8, false);
  		for (int CounterX = 0; CounterX < heft0_1.GetLength(0); ++CounterX)
  		{
  			for (int CounterY = 0; CounterY < heft0_1.GetLength(1); ++CounterY)
  			{
  				writer.Write(heft0_1[CounterX, CounterY]);
  			}
  		}
  		for (int CounterX = 0; CounterX < heft1_2.GetLength(0); ++CounterX)
  		{
  			for (int CounterY = 0; CounterY < heft1_2.GetLength(1); ++CounterY)
  			{
  				writer.Write(heft1_2[CounterX, CounterY]);
  			}
  		}
  		for (int CounterX = 0; CounterX < heft2_3.GetLength(0); ++CounterX)
  		{
  			for (int CounterY = 0; CounterY < heft2_3.GetLength(1); ++CounterY)
  			{
  				writer.Write(heft2_3[CounterX, CounterY]);
  			}
  		}
  		writer.Write(AllSum);
  		writer.Write(LearningRate);
  	}
  	public static void DisplayValues(double[,] heft0_1, double[,] heft1_2, double[,] heft2_3, ref double BestWay, ref double LearningRate)
  	{
  		if (File.Exists(fileName))
  		{
  			using FileStream stream = File.Open(fileName, FileMode.Open);
  			using BinaryReader reader = new BinaryReader(stream, Encoding.UTF8, false);

  			for (int CounterX = 0; CounterX < heft0_1.GetLength(0); ++CounterX)
  			{
  				for (int CounterY = 0; CounterY < heft0_1.GetLength(1); ++CounterY)
  				{
  					heft0_1[CounterX, CounterY] = reader.ReadDouble();
  				}
  			}

  			for (int CounterX = 0; CounterX < heft1_2.GetLength(0); ++CounterX)
  			{
  				for (int CounterY = 0; CounterY < heft1_2.GetLength(1); ++CounterY)
  				{
  					heft1_2[CounterX, CounterY] = reader.ReadDouble();
  				}
  			}

  			for (int CounterX = 0; CounterX < heft2_3.GetLength(0); ++CounterX)
  			{
  				for (int CounterY = 0; CounterY < heft2_3.GetLength(1); ++CounterY)
  				{
  					heft2_3[CounterX, CounterY] = reader.ReadDouble();
  				}
  			}

  			BestWay = reader.ReadDouble();
  			LearningRate = reader.ReadDouble();
  		}
  	}
  }
  public static class Extensions
  {
  	public static int ReadBigInt32(this BinaryReader br)
  	{
  		byte[] bytes = br.ReadBytes(sizeof(int));
  
  		if (BitConverter.IsLittleEndian)
  		{
  			Array.Reverse(bytes);
  		}
  
  		return BitConverter.ToInt32(bytes, 0);
  	}
  	public static void ForEach<T>(this T[,] source, Action<int, int> action)
  	{
  		for (int CounterX = 0; CounterX < source.GetLength(0); CounterX++)
  		{
  			for (int CounterY = 0; CounterY < source.GetLength(1); CounterY++)
  			{
  				action(CounterX, CounterY);
  			}
  		}
  	}
  }
  public class Image
  {
  	public byte Label   { get; set; }
  	public byte[,] Data { get; set; }
  }
  public static class MnistReader
  {
  	private const string TrainImages = "mnist/train-images.idx3-ubyte";
  	private const string TrainLabels = "mnist/train-labels.idx1-ubyte";
  	private const string TestImages  = "mnist/t10k-images.idx3-ubyte";
  	private const string TestLabels  = "mnist/t10k-labels.idx1-ubyte";
  	public static IEnumerable<Image> ReadTrainingData()
  	{
  		foreach (var item in Read(TrainImages, TrainLabels))
  		{
  			yield return item;
  		}
  	}
  	public static IEnumerable<Image> ReadTestData()
  	{
  		foreach (var item in Read(TestImages, TestLabels))
  		{
  			yield return item;
  		}
  	}
  	private static IEnumerable<Image> Read(string imagesPath, string labelsPath)
  	{
  		using BinaryReader labels = new BinaryReader(new FileStream(labelsPath, FileMode.Open));
  		using BinaryReader images = new BinaryReader(new FileStream(imagesPath, FileMode.Open));
  
  		int magicNumber = images.ReadBigInt32();
  		int numberOfImages = images.ReadBigInt32();
  		int width = images.ReadBigInt32();
  		int height = images.ReadBigInt32();
  
  		int magicLabel = labels.ReadBigInt32();
  		int numberOfLabels = labels.ReadBigInt32();
  
  		for (int Counter = 0; Counter < numberOfImages; Counter++)
  		{
  			var bytes = images.ReadBytes(width * height);
  			var arr = new byte[height, width];
  
  			arr.ForEach((CounterX, CounterY) => arr[CounterX, CounterY] = bytes[CounterX * height + CounterY]);
  
  			yield return new Image()
  			{
  				Data  = arr,
  				Label = labels.ReadByte()
  			};
  		}
  	}
  }
  class Program
  {
  	public static void GenerationRandomheft(double[,] arr)
  	{
  		Random rnd = new Random();
  		for (int CounterX = 0; CounterX < arr.GetLength(0); ++CounterX)
  		{
  			for (int CounterY = 0; CounterY < arr.GetLength(1); ++CounterY)
  			{
  				 arr[CounterX,CounterY] = rnd.NextDouble();
  			}
  		}
  	}
  	public static void ForWards(double[,] L1, double[,] L2, double[,] heft)
  	{  
  		for (int CounterY = 0; CounterY < heft.GetLength(1); ++CounterY)
  		{
  			L2[CounterY,0] = 0;
  			for (int CounterX = 0; CounterX < heft.GetLength(0); ++CounterX)
  			{
  				L2[CounterY, 0] = L2[CounterY, 0] + (L1[CounterX, 0] * heft[CounterX,CounterY]);
  			}
  			L2[CounterY, 0] = 1 / (1 + Math.Pow(Math.E, -1 * L2[CounterY, 0]));
  		}
  	}
  	public static void FindError(double[,] L1, double[,] L2, double[,] heft)
  	{
  		for (int CounterX = 0; CounterX < heft.GetLength(0); ++CounterX)
  		{
  			L1[CounterX, 1] = 0;
  			for (int CounterY = 0; CounterY < heft.GetLength(1); ++CounterY)
  			{
  				L1[CounterX, 1] = L1[CounterX, 1] + (L2[CounterY, 1] * heft[CounterX, CounterY]);
  			}
  		}
  	}
  	public static void FixOutError(ref byte IDL, double[,] L3)
  	{
  		for (int Counter = 0; Counter < L3.GetLength(0) ;++Counter)
  		{
  			L3[Counter, 1] = Counter == IDL ? 1 - L3[Counter, 0] : 0 - L3[Counter, 0];
  		}
  	}
  	public static void BackWards(double[,] L1, double[,] L2, double[,] heft, double LearningRate)
  	{
  		for (int CounterY = 0; CounterY < heft.GetLength(1); ++CounterY)
  		{
  			for (int CounterX = 0; CounterX < heft.GetLength(0); ++CounterX)
  			{
  				heft[CounterX, CounterY] = heft[CounterX, CounterY] + (LearningRate * L2[CounterY, 1] * (L2[CounterY, 0] * (1 - L2[CounterY, 0])) * L1[CounterX, 0]);
  			}
  		}
  	}
  	public static void GetTask(Image image, double[,] L0, ref byte CorrectАnswer)
  	{
  		CorrectАnswer = image.Label;
  		int Counter = 0;
  		for (int CounterX = 0; CounterX < image.Data.GetLength(0); ++CounterX)
  		{
  			for (int CounterY = 0; CounterY < image.Data.GetLength(0); ++CounterY)
  			{
  				L0[Counter, 0] = image.Data[CounterX,CounterY] / 255.0;
  				++Counter;
  			}
  		}
  	}
  	public static void SumError(ref double[,] L3,ref double Sum,byte label)
  	{
  		for (int Counter = 0; Counter < L3.GetLength(0); ++Counter)
  		{
  			if (label == Counter)
  			{
  				Sum += L3[Counter, 1];
  			}
  		}
  	}
  	static void Main(string[] args)
  	{
  		double[,] heft0_1 = new double[784,16];
  		double[,] heft1_2 = new double[16,16];
  		double[,] heft2_3 = new double[16,10];
  
  		double[,] L0 = new double[784, 2];
  		double[,] L1 = new double[16,2];
  		double[,] L2 = new double[16,2];
  		double[,] L3 = new double[10,2];
  
  		double LearningRate = 1;
  
  		byte CorrectАnswer = 0;
  
  		double Sum = 0;
  		double AllSum = 0;
  		double BestWay = 100;
  		double NewWay = 0;
  
  		if (false)
  		{
  			GenerationRandomheft(heft0_1);
  			GenerationRandomheft(heft1_2);
  			GenerationRandomheft(heft2_3);
  			ConsoleApplication.WriteDefaultValues(heft0_1, heft1_2, heft2_3, BestWay, LearningRate);
  		}
  
  		ConsoleApplication.DisplayValues(heft0_1, heft1_2, heft2_3, ref BestWay, ref LearningRate);
  		while (true)
  		{
  			foreach (var image in MnistReader.ReadTrainingData())
  			{
  				GetTask(image, L0, ref CorrectАnswer);
  
  				ForWards(L0, L1, heft0_1);
  				ForWards(L1, L2, heft1_2);
  				ForWards(L2, L3, heft2_3);
  
  				FixOutError(ref CorrectАnswer, L3);
  				FindError(L2, L3, heft2_3);
  				FindError(L1, L2, heft1_2);
  
  				BackWards(L2, L3, heft2_3, LearningRate);
  				BackWards(L1, L2, heft1_2, LearningRate);
  				BackWards(L0, L1, heft0_1, LearningRate);
  
  				SumError(ref L3, ref Sum, image.Label);
  				AllSum += Sum;
  				Sum = 0;
  			}

        NewWay = AllSum / 600;
        AllSum = 0;
        Console.WriteLine(NewWay + " %");
  
  			if ((BestWay >= NewWay) & (BestWay != 0))
  			{
  				BestWay = NewWay;

  				ConsoleApplication.WriteDefaultValues(heft0_1, heft1_2, heft2_3, BestWay, LearningRate);
  				Console.WriteLine("Лучший резутьтат был записан. LearningRate = " + LearningRate);
  			} else {
          LearningRate -= 0.01;

          if (LearningRate <= 0)
          {
            LearningRate = 1;
          }
          double i = 0; //Временная заглушка, пока не исправлю
          ConsoleApplication.DisplayValues(heft0_1, heft1_2, heft2_3,ref BestWay, ref i);
  				Console.WriteLine("Результат стал хуже. Меняем коэффициент обучения. LearningRate = " + LearningRate + ". Лучший результат = " + BestWay + " %");
  			}
  		}
  	}
  }
}
