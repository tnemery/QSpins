using System;
using UnityEngine;
using org.opensourcephysics.orst.spins; // allows access to the Complex.cs functions
using SpinVectorN;

namespace UniMat
{

	public class Matrix
	{

	  public Complex[][] data = RectangularArrays.ReturnRectangularComplexArray(3, 3);

	  public Matrix()
	  {
		for (int i = 0; i < 3; i++)
		{
		  for (int j = 0; j < 3; j++)
		  {
			this.data[i][j] = new Complex(0.0D, 0.0D);
		  }
		}
	  }

	public virtual Matrix Identity(){
			Matrix localMatrix = new Matrix();
			for(int i = 0; i<3;i++){
				for(int j = 0; j<3; j++){
					if(i == j){
						localMatrix.data[i][j] = new Complex(1,0);
					}
					else{
						localMatrix.data[i][j] = new Complex(0,0);
					}
				}
			}
			return localMatrix;
	}

	  public virtual Matrix mAdd(Matrix paramMatrix)
	  {
		Matrix localMatrix = new Matrix();
		for (int i = 0; i < 3; i++)
		{
		  for (int j = 0; j < 3; j++)
		  {
			localMatrix.data[i][j] = this.data[i][j].add(paramMatrix.data[i][j]);
		  }
		}
		return localMatrix;
	  }

	  public virtual Matrix mMul(Complex paramComplex)
	  {
			Matrix localMatrix = new Matrix();
		for (int i = 0; i < 3; i++)
		{
		  for (int j = 0; j < 3; j++)
		  {
			localMatrix.data[i][j] = paramComplex.product(this.data[i][j]);
		  }
		}
		return localMatrix;
	  }


		public virtual SpinVector mvMul(SpinVector paramSpinVector)
	  {
		SpinVector localSpinVector = new SpinVector();
		for (int i = 0; i < 3; i++)
		{
		  for (int j = 0; j < 3; j++)
		  {
				localSpinVector.data[i] = localSpinVector.data[i].add (paramSpinVector.data[j].product(this.data[i][j]));
		  }
		}
		return localSpinVector;
	  } 

	  public virtual Matrix SquareMatrix()
	  {
			Matrix localMatrix = new Matrix();
		for (int i = 0; i < 3; i++)
		{
		  for (int j = 0; j < 3; j++)
		  {
			for (int k = 0; k < 3; k++)
			{
			  localMatrix.data[i][j] = localMatrix.data[i][j].add(this.data[i][k].product(this.data[k][j]));
			}
		  }
		}
		return localMatrix;
	  }

		/*
		public virtual void ComputeU()
		{
			double d = 6.283185307179586D * this.number / 72.0D;
			Matrix localMatrix = this.experiment.oper[Op].mMul(new Complex(0.0D, -Math.Sin(d)));
			localMatrix = localMatrix.mAdd(Experiment.identity);
			this.UMatrix = this.experiment.opSquared[Op].mMul(new Complex(Math.Cos(d) - 1.0D, 0.0D));
			this.UMatrix = localMatrix.mAdd(this.UMatrix);
		} */

		  public virtual void printMatrix()
		  {
			for (int i = 0; i < 3; i++)
			{
			  for (int j = 0; j < 3; j++)
			  {
					Debug.Log("data["+i+"]["+j+"] = ");
				this.data[i][j].printComplex();
			  }
			}
		  } 
	}
}

//----------------------------------------------------------------------------------------
//	Copyright © 2007 - 2015 Tangible Software Solutions Inc.
//	This class can be used by anyone provided that the copyright notice remains intact.
//
//	This class provides the logic to simulate Java rectangular arrays, which are jagged
//	arrays with inner arrays of the same length. A size of -1 indicates unknown length.
//----------------------------------------------------------------------------------------
internal static partial class RectangularArrays
{
    internal static Complex[][] ReturnRectangularComplexArray(int size1, int size2)
    {
        Complex[][] newArray;
        if (size1 > -1)
        {
            newArray = new Complex[size1][];
            if (size2 > -1)
            {
                for (int array1 = 0; array1 < size1; array1++)
                {
                    newArray[array1] = new Complex[size2];
                }
            }
        }
        else
            newArray = null;

        return newArray;
    }
}