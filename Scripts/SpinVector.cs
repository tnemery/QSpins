using System;
using UnityEngine;
using System.Collections;
using org.opensourcephysics.orst.spins; // allows access to the Complex.cs functions


namespace SpinVectorN
{

	public class SpinVector {
		public Complex[] data = new Complex[3];
		
		public SpinVector()
		{
			for (int i = 0; i < 3; i++)
			{
				this.data[i] = new Complex(0.0D, 0.0D);
			}
		}


		public Complex dotProduct(SpinVector paramSpinVector)
		{
			Complex localComplex = new Complex(0.0D, 0.0D);
			for (int i = 0; i < 3; i++) {
				localComplex = localComplex.add(this.data[i].conjugation().product(paramSpinVector.data[i]));
			}
			return localComplex;
		}

		public SpinVector normalize()
		{
			Complex localComplex = dotProduct(this);
			localComplex = new Complex(1.0D / Mathf.Sqrt((float)localComplex.RPart), 0.0D);
			return vMul(localComplex);
		}

		public SpinVector vMul(Complex paramComplex)
		{
			SpinVector localSpinVector = new SpinVector();
			for (int i = 0; i < 3; i++) {
				localSpinVector.data[i] = paramComplex.product(this.data[i]);
			}
			return localSpinVector;
		}

		public void printVector()
		{
			for (int i = 0; i < 3; i++)
			{
				Debug.Log("data["+i+"] = ");
				this.data[i].printComplex();
			}
		}

	}
}
