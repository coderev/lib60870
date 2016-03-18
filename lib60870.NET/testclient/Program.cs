using System;
using System.Threading;

using lib60870;

namespace testclient
{
	class MainClass
	{

		private static bool asduReceivedHandler(object parameter, ASDU asdu)
		{
			Console.WriteLine (asdu.ToString ());

			if (asdu.TypeId == TypeID.M_SP_NA_1) {

				for (int i = 0; i < asdu.NumberOfElements; i++) {

					var val = (SinglePointInformation)asdu.GetElement (i);

					Console.WriteLine ("  IOA: " + val.ObjectAddress + " SP value: " + val.Value);
					Console.WriteLine ("   " + val.Quality.ToString ());
				}
			} 
			else if (asdu.TypeId == TypeID.M_ME_TE_1) {
			
				for (int i = 0; i < asdu.NumberOfElements; i++) {

					var msv = (MeasuredValueScaledWithCP56Time2a)asdu.GetElement (i);

					Console.WriteLine ("  IOA: " + msv.ObjectAddress + " scaled value: " + msv.ScaledValue);
					Console.WriteLine ("   " + msv.Quality.ToString ());
					Console.WriteLine ("   " + msv.Timestamp.ToString ());
				}

			} else if (asdu.TypeId == TypeID.M_ME_TF_1) {

				for (int i = 0; i < asdu.NumberOfElements; i++) {
					var mfv = (MeasuredValueShortFloatWithCP56Time2a)asdu.GetElement (i);

					Console.WriteLine ("  IOA: " + mfv.ObjectAddress + " float value: " + mfv.Value);
					Console.WriteLine ("   " + mfv.Quality.ToString ());
					Console.WriteLine ("   " + mfv.Timestamp.ToString ());
					Console.WriteLine ("   " + mfv.Timestamp.GetDateTime ().ToString ());
				}
			} else if (asdu.TypeId == TypeID.M_SP_TB_1) {

				for (int i = 0; i < asdu.NumberOfElements; i++) {

					var val = (SinglePointWithCP56Time2a)asdu.GetElement (i);

					Console.WriteLine ("  IOA: " + val.ObjectAddress + " SP value: " + val.Value);
					Console.WriteLine ("   " + val.Quality.ToString ());
					Console.WriteLine ("   " + val.Timestamp.ToString ());
				}
			} else if (asdu.TypeId == TypeID.M_ME_NC_1) {

				for (int i = 0; i < asdu.NumberOfElements; i++) {
					var mfv = (MeasuredValueShortFloat)asdu.GetElement (i);

					Console.WriteLine ("  IOA: " + mfv.ObjectAddress + " float value: " + mfv.Value);
					Console.WriteLine ("   " + mfv.Quality.ToString ());
				}
			} else if (asdu.TypeId == TypeID.M_ME_NB_1) {

				for (int i = 0; i < asdu.NumberOfElements; i++) {

					var msv = (MeasuredValueScaled)asdu.GetElement (i);

					Console.WriteLine ("  IOA: " + msv.ObjectAddress + " scaled value: " + msv.ScaledValue);
					Console.WriteLine ("   " + msv.Quality.ToString ());
				}

			} else {
				Console.WriteLine ("Unknown message type!");
			}

			return true;
		}

		public static void Main (string[] args)
		{

			Connection con = new Connection ("192.168.1.50");
			//Connection con = new Connection ("127.0.0.1");

			con.SetASDUReceivedHandler (asduReceivedHandler, null);

			con.connect ();

			Thread.Sleep (5000);

			con.sendInterrogationCommand (CauseOfTransmission.ACTIVATION, 1, 20);

			Thread.Sleep (5000);

			con.sendControlCommand (TypeID.C_SC_NA_1, CauseOfTransmission.ACTIVATION, 1, new SingleCommand (5000, true, false, 0));

			con.sendControlCommand (TypeID.C_DC_NA_1, CauseOfTransmission.ACTIVATION, 1, new DoubleCommand (5001, DoubleCommand.ON, false, 0));

			con.sendControlCommand (TypeID.C_RC_NA_1, CauseOfTransmission.ACTIVATION, 1, new StepCommand (5002, StepCommand.HIGHER, false, 0));

			con.sendControlCommand (TypeID.C_SC_TA_1, CauseOfTransmission.ACTIVATION, 1, 
			                        new SingleCommandWithCP56Time2a (5000, false, false, 0, new CP56Time2a (DateTime.Now)));

		}
	}
}