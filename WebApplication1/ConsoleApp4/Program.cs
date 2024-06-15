// See https://aka.ms/new-console-template for more information
using ClassLibrary1;

Console.WriteLine("Hello, World!");
TrackList trackList = new TrackList();
trackList.AddNewTrack("newTrack");
bool result = trackList.CheckByName("newTrac");
Console.WriteLine(result);