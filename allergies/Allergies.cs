using System;
using System.Collections.Generic;
using System.Linq;

public enum Allergen
{
	Eggs=0x1,
	Peanuts=0x2,
	Shellfish=0x4,
	Strawberries=0x8,
	Tomatoes=0x10,
	Chocolate=0x20,
	Pollen=0x40,
	Cats=0x80,
}

public class Allergies
{
	public List<Allergen> List() =>
		(from Allergen allergen in Enum.GetValues(typeof(Allergen))
		 where AllergyScore.HasFlag(allergen)
		 select allergen).ToList();

	public Allergen AllergyScore { get; set; }

	public Allergies(int allergyScore)
	{
		AllergyScore = (Allergen)allergyScore;
	}

	public bool IsAllergicTo(Allergen allergen) =>
		 AllergyScore.HasFlag(allergen);
}
