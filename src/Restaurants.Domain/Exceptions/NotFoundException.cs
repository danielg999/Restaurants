namespace Restaurants.Domain.Exceptions;

public class NotFoundException(string objectType, string objectIdentifier) 
    : Exception ($"{objectType} with id: {objectIdentifier} doesn't exist")
{

}
