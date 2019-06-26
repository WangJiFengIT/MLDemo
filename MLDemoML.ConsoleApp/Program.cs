//*****************************************************************************************
//*                                                                                       *
//* This is an auto-generated file by Microsoft ML.NET CLI (Command-Line Interface) tool. *
//*                                                                                       *
//*****************************************************************************************

using System;
using System.IO;
using System.Linq;
using Microsoft.ML;
using MLDemoML.Model.DataModels;


namespace MLDemoML.ConsoleApp
{
    class Program
    {
        //����ѧϰģ�ͣ����ڼ��غ�Ԥ��
        private const string MODEL_FILEPATH = @"MLModel.zip";

        //����Ԥ������ݼ�
        private const string DATA_FILEPATH = @"C:\Users\wangjifeng\Desktop\data.tsv";

        static void Main(string[] args)
        {
            MLContext mlContext = new MLContext();

            //��ML.NET CLI��AutoML��������ģ�͵�ѵ������
            //ModelBuilder.CreateModel();

            ITransformer mlModel = mlContext.Model.Load(GetAbsolutePath(MODEL_FILEPATH), out DataViewSchema inputSchema);
            var predEngine = mlContext.Model.CreatePredictionEngine<ModelInput, ModelOutput>(mlModel);

            // ����ʾ��������������е���Ԥ��
            ModelInput sampleData = CreateSingleDataSample(mlContext, DATA_FILEPATH);

            // ����һ��Ԥ��
            ModelOutput predictionResult = predEngine.Predict(sampleData);

            Console.WriteLine($"Single Prediction --> Actual value: {sampleData.Sentiment} | Predicted value: {predictionResult.Prediction}");

            Console.WriteLine("=============== End of process, hit any key to finish ===============");
            Console.ReadKey();
        }

        // �������ص��������Գ��Ե���Ԥ��
        // ������޸���δ��룬�������ﴴ�����Լ�����������(Ӳ����Ļ������κ���Դ��)
        private static ModelInput CreateSingleDataSample(MLContext mlContext, string dataFilePath)
        {
            // ��ȡdataset�Ի�����ڳ���Ԥ��ĵ���      
            IDataView dataView = mlContext.Data.LoadFromTextFile<ModelInput>(
                                            path: dataFilePath,
                                            hasHeader: true,
                                            separatorChar: '\t',
                                            allowQuoting: true,
                                            allowSparse: false);

            // ������(ModelInput����)���������ṩ�µĲ������ݣ�������Ӳ����Ļ������������û�Ӧ�ó���ģ������������ļ����С�
            ModelInput sampleForPrediction = mlContext.Data.CreateEnumerable<ModelInput>(dataView, false)
                                                                        .First();
            return sampleForPrediction;
        }

        public static string GetAbsolutePath(string relativePath)
        {
            FileInfo _dataRoot = new FileInfo(typeof(Program).Assembly.Location);
            string assemblyFolderPath = _dataRoot.Directory.FullName;

            string fullPath = Path.Combine(assemblyFolderPath, relativePath);

            return fullPath;
        }
    }
}
