using ExampleTest2.Models;

namespace ExampleTest2.Services;

public interface IDbService
{
    Task<bool> CharacterExists(int characterId);
    Task<Character> GetCharacterData(int characterId);

    Task<bool> ItemsExist(List<int> itemIds);

    Task<List<Item>> GetItems(List<int> itemIds);

    Task AddBackpackItems(List<Backpack> items);
    // Task<bool> PatientExists(int patientId);
    // Task<bool> DoctorExists(int doctorId);
    //
    // Task NewPatient(Patient patient);
    // Task NewPrescription(Prescription patient);
    // Task NewPrescriptionMedicaments(List<PrescriptionMedicament> prescriptionMedicaments);
    //
    // Task<List<Prescription>> GetPrescriptionsForPatient(int idPatient);
    //
    // Task<Patient> GetPatientData(int idPatient);
}