/*TASK SerializableInterface*/
//                                                          //AUTHOR: Towa (RPM-Ruben de la Pena, JJFM-Juan Jose Flores,
//                                                          //      LGCR-Leoncio Chiunty).
//                                                          //CO-AUTHOR: Towa (GLG-Gerardo Lopez).
//                                                          //DATE: August 31, 2019.

namespace TowaStandard
{
    //==================================================================================================================
    public interface SerializableInterface<TSerializable>
    {
        //                                                  //Bytes serialization. (very efficient and compact).
        //                                                  //All objects to be serialized should implement this
        //                                                 //       interface

        //--------------------------------------------------------------------------------------------------------------
        byte[] Serialize(
            //                                              //Get a serialized version of the object.
            );

        //--------------------------------------------------------------------------------------------------------------
        void Deserialize(
            //                                              //Returns a deserialized object.

            //                                              //The object to deserialize.
            out TSerializable Serialazable_O,
            //                                              //The serialized bytes.
            ref byte[] Bytes_IO
            );

        //--------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
