using System;
using UnityEngine;

namespace org.opensourcephysics.orst.spins
{

	public class Complex
	{
	  private double rPart;
	  private double iPart;

	  public Complex(double paramDouble1, double paramDouble2)
	  {
		this.rPart = paramDouble1;
		this.iPart = paramDouble2;
	  }

	  public virtual double RPart
	  {
		  get
		  {
			return this.rPart;
		  }
	  }

	  public virtual double IPart
	  {
		  get
		  {
			return this.iPart;
		  }
	  }

	  public virtual Complex conjugation()
	  {
			return new Complex(this.rPart, -this.iPart);
	  }

	  public virtual Complex add(Complex paramComplex)
	  {
		double d1 = this.rPart + paramComplex.RPart;
		double d2 = this.iPart + paramComplex.IPart;
		return new Complex(d1, d2);
	  }

	  public virtual Complex subtract(Complex paramComplex)
	  {
		double d1 = this.rPart - paramComplex.RPart;
		double d2 = this.iPart - paramComplex.IPart;
		return new Complex(d1, d2);
	  }

	  public virtual Complex product(Complex paramComplex)
	  {
		double d1 = this.rPart * paramComplex.RPart - this.iPart * paramComplex.IPart;
		double d2 = this.iPart * paramComplex.RPart + this.rPart * paramComplex.IPart;
		return new Complex(d1, d2);
	  }

	  public virtual Complex scale(double paramDouble)
	  {
		return new Complex(this.rPart * paramDouble, this.iPart * paramDouble);
	  }

	  public virtual double modSquared()
	  {
		return this.rPart * this.rPart + this.iPart * this.iPart;
	  }

	  public virtual double mod()
	  {
		return Math.Sqrt(modSquared());
	  }

	  public virtual Complex divide(Complex paramComplex)
	  {
		return product(paramComplex.conjugation()).scale(1.0D / paramComplex.modSquared());
	  }

	  public virtual double radiam()
	  {
		if (this.rPart == 0.0D)
		{
		  if (this.iPart == 0.0D)
		  {
			return (0.0D / 0.0D);
		  }
		  if (this.iPart > 0.0D)
		  {
			return 1.570796326794897D;
		  }
		  return -1.570796326794897D;
		}
		return Math.Atan(this.iPart / this.rPart);
	  }

	  public virtual void printComplex()
	  {
			Debug.Log(RPart+" + "+IPart+" i");
	  }
	}

}