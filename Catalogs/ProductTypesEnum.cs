/*TASK RP. RESOURCES*/

//                                                          //AUTHOR: Towa (VSTD - Victor Torres).
//                                                          //CO-AUTHOR: Towa (LGF-Liliana Gutierrez).
//                                                          //DATE: November 19, 2019.

//======================================================================================================================
enum ProductTypeEnum
{
    //                                                      //Default 
    None,
    //                                                      //The last page or sheet of a softcover book or magazine, 
    //                                                      //      commonly a heavier media.
    BackCover,
    //                                                      //Cut, Unfolded box, input for folder-gluer.
    BlankBox,
    //                                                      //An unprinted divider page or sheet. Also describes 
    //                                                      //      die-cut unprinted label.
    BlankSheet,
    //                                                      //A web with connected blanks after a die cutting.
    BlankWeb,
    //                                                      //Generic content inside of a cover, e.g. "BookBlock". 
    Body,
    //                                                      //Body with a cover and a spine.
    Book,
    //                                                      //The assembled body of pages for a hardcover book.
    BookBlock,
    //                                                      //The assembled covers and spine component of a hardcover 
    //                                                      //      book, prior to "casing in" (attaching to the 
    //                                                      //      book block).
    BookCase,
    //                                                      //Body with a cover without a spine (typically stapled).
    Booklet,
    //                                                      //Convenience packaging that is not envisioned to be 
    //                                                      //      protection for shipping. 
    Box,
    //                                                      //A single folded sheet.
    Brochure,
    //                                                      //A small card that displays contact information for an 
    //                                                      //      individual employed by a company.
    BusinessCard,
    //                                                      //Protection packaging for shipping.
    Carton,
    //                                                      //A single sheet covering a side of a print product.
    Cover,
    //                                                      //A letter accompanying another print product.
    CoverLetter,
    //                                                      //A glued sheet that spans and attaches BookBlock to 
    //                                                      //      BookCase, in both front and back of a hardcover 
    //                                                      //      book, (printed or not).
    EndSheet,
    //                                                      //A folded paper container, with sealable flap, that 
    //                                                      //      encloses and protects a document or contents.
    Envelope,
    //                                                      //A folded and glued blank (not opened). Output from a box 
    //                                                      //      folder-gluer. 
    FlatBox,
    //                                                      //Non-bound, non-folded products or products that only have 
    //                                                      //      packaging folds.
    FlatWork,
    //                                                      //The first page or sheet of a softcover book or magazine, 
    //                                                      //      commonly a heavier media.
    FrontCover,
    //                                                      //A product part intended to be inserted into a print 
    //                                                      //      product.
    Insert,
    //                                                      //Hardcover case jacket.
    Jacket,
    //                                                      //A piece of paper or plastic that is attached to an object 
    //                                                      //      in order to give information about it.
    Label,
    //                                                      //A single unfolded sheet.
    Leaflet,
    //                                                      //A written or printed communication addressed to a person 
    //                                                      //      or organization and usually transmitted by mail 
    //                                                      //      or messenger.
    Letter,
    //                                                      //A drawing/representation of a particular area such as a 
    //                                                      //      city, or a continent, showing its main features, 
    //                                                      //      as they would appear if viewed from above.
    Map,
    //                                                      //Unprinted media, the substrate (usually paper) on which 
    //                                                      //      an image is to be printed.
    Media,
    //                                                      //A newspaper-product.
    Newspaper,
    //                                                      //A book or block with a set of identical or similar pages, 
    //                                                      //      e.g. a writing tablet, where all page fronts have 
    //                                                      //      identical content, and all page backs have identical 
    //                                                      //      content.
    Notebook,
    //                                                      //Loaded pallet of boxes, cartons or Component resources.
    Pallet,
    //                                                      //A card designed for sending a message by mail without 
    //                                                      //      an envelope
    Postcard,
    //                                                      //A large printed picture.
    Poster,
    //                                                      //A representation that visualizes the intended output of 
    //                                                      //      page assembly, or the printing process.
    Proof,
    //                                                      //A self mailer to respond to an offer.
    ResponseCard,
    //                                                      //Main division of a book, such as a chapter, typically 
    //                                                      //      with a name or number.
    Section,
    //                                                      //A document to be sent via the post without an additional 
    //                                                      //      envelope.
    SelfMailer,
    //                                                      //The bound edge of a book. Also, the portion of the cover 
    //                                                      //      that connects the front and back cover, wrapping 
    //                                                      //      the binding edge.
    Spine,
    //                                                      //Stacked Component.
    Stack,
    //                                                      //A single cover sheet containing the front cover, spine 
    //                                                      //      and back cover
    WrapAroundCover
}

//======================================================================================================================

/*END-TASK*/
