//----------------------------------------------------------
// Copyright (C) JumbleBlocks. All rights reserved.
//----------------------------------------------------------
/// <reference path="jquery-1.7.2.js" />

//TODO: Check http://encosia.com/automatically-minify-and-combine-javascript-in-visual-studio/
//TODO: check http://www.codeproject.com/Articles/34035/Combine-Compress-Minify-JS-and-CSS-files-in-ASP-NE

//TODO: comment code appropriatly
//TODO: see if can make event names static

//GLOBAL JQUERY EXTENSION FUNCTION

jQuery.jumbleblocks = {

    //image viewer plugin
    imageViewer: {

        //default values to use if no attributes specified
        defaultUrl: '/Image/ImageList',
        urlAttributeName: 'ImageRetrivalUrl',

        //Creates image viewer within specified element
        createImageViewerWithin: function ($element) {

            if (!$element.is("div"))
                throw new Error("ImageViewer element must be a div");
            else {
                var imageViewer = $element.createOrReturnJumbleBlocksImageViewer();
                imageViewer.loadAndShowImagesForPage(1);
                return imageViewer;
            }
        },

        getImageViewerIn: function ($element) {
            return $element.getImageViewer();
        }
    }
}

 var IMAGEVIEWER_DATAKEY = "Jumbleblocks_ImageViewer";


//JQUERY FUNCTION TO CREATE IMAGEVIEWER OBJECT ON ELEMENT
 jQuery.fn.createOrReturnJumbleBlocksImageViewer = function () {
     var imageViewerObj = this.getImageViewer();

     if (typeof imageViewerObj === 'undefined') {
         imageViewerObj = new ImageViewer($(this));
         $(this).data(IMAGEVIEWER_DATAKEY, imageViewerObj);
     }

     return imageViewerObj;
 }

 jQuery.fn.getImageViewer = function () {
     var imageViewerObj = $(this).data(IMAGEVIEWER_DATAKEY);
     return imageViewerObj;
 }


//IMAGEVIEWER CLASS
//$containerElement - jQuery element representing div container
function ImageViewer($containerElement) {
    EventDispatchManager.call(this);
    var self = this;
    this.$containerElement = $containerElement;

    this.page = null;
    this.pagerController = null;
    this.imageLoader = null;

    this.selectedImage = null;

    this.ImageSelectedEventName = "ImageViewer_ImageSelected";

    var initialize = function () {
        createPage();
        createImageLoader();
        createPagerController();
        registerEventHandlers();
        registerEventTypes();
    };

    var createPage = function () {
        var $pageElement = self.$containerElement.find('#ImageViewerPage:first');

        if (typeof $pageElement === 'undefined') {
            throw new Error("#ImageViewer does not have a child with the id: #ImageViewerPage");
        }

        self.page = new Page($pageElement);
    };

    var createImageLoader = function () {
        var numberOfImagesPerPage = self.getNumberOfImagesPerPage();

        self.imageLoader = new ImageLoader(numberOfImagesPerPage, self.getUrlToRetrieveImages());
    };

    var createPagerController = function () {
        var leftPager = createIncrementPager('Left');
        var rightPager = createIncrementPager('Right');
        var numberPager = createNumberPager();
        var numberOfImagesPerPage = self.getNumberOfImagesPerPage();

        self.pagerController = new PagerController(1, numberOfImagesPerPage, leftPager, rightPager, numberPager);
    };

    var createIncrementPager = function (direction) {
        var upperCaseDirection = direction.toUpperCase();

        if (upperCaseDirection != 'LEFT' && upperCaseDirection != 'RIGHT') {
            throw new Error("direction must be left or right");
        }

        var $incrementPagerElement = self.$containerElement.find("#ImageViewerPager" + direction + ":first");

        return new IncrementPager($incrementPagerElement, upperCaseDirection);
    };

    var createNumberPager = function () {
        var $numberPagerElement = self.$containerElement.find("#ImageViewerAvailablePages:first");
        return new NumberedPager($numberPagerElement);
    };

    var registerEventHandlers = function () {
        self.imageLoader.registerListener(self.imageLoader.ImagesRetrievedEventName, function (evt) {
            self.showImagePage(evt.imageList);
            self.pagerController.setCurrentPageNumber(evt.pageNumber);
        });

        self.imageLoader.registerListener(self.imageLoader.ImageRetrievalFailedEventName, function (evt) {
            self.errorRetrievingImages(evt.status, evt.error);
        });

        self.pagerController.registerListener(self.pagerController.ChangePageEventName, function (evt) {
            self.loadAndShowImagesForPage(evt.pageNumber);
        });

        self.page.registerListenerForBoxSelectedEvent(imageSelectedHandler);
    };

    var registerEventTypes = function () {
        self.registerEventType(new EventType({ name: self.ImageSelectedEventName }));
    };

    this.loadAndShowImagesForPage = function (pageNumber) {
        this.imageLoader.loadImagesForPage(pageNumber);
    };

    //gets number of images allowed on each page
    this.getNumberOfImagesPerPage = function () {
        return this.page.getTotalBoxCount();
    };

    //gets url to retrieve images on
    this.getUrlToRetrieveImages = function () {
        var urlAttribName = jQuery.jumbleblocks.imageViewer.urlAttributeName;

        var url = this.$containerElement.attr(urlAttribName);

        if (url !== undefined)
            return specifiedUrl;
        else
            return jQuery.jumbleblocks.imageViewer.defaultUrl;
    };

    //shows a new page of images
    this.showImagePage = function (imageList) {
        if (imageList.ReturnedCount > 0) {

            var imageWrappers = convertImageListToListOfImageWrapper(imageList.ImageViewModels);
            this.page.updateImages(imageWrappers);
            this.pagerController.updateForTotalNumberOfImages(imageList.TotalCount);

        }
        else {
            this.noMoreImages();
        }
    };

    var convertImageListToListOfImageWrapper = function (imageList) {
        var wrappers = [];

        $.each(imageList, function (index, object) {
            var imageWrapper = new ImageWrapper(object.Id, object.Url);
            wrappers.push(imageWrapper);
        });

        return wrappers;
    };

    this.noMoreImages = function () {
        //TODO: show error correctly
        alert('no more images - not handled');
    };

    this.errorRetrievingImages = function (status, error) {
        //TODO: show error correctly
        alert(error);
        alert(status);
    };

    var getCurrentPageNumber = function(){
        return self.pagerController.currentPageNumber;
    };

    var imageSelectedHandler = function (evt) {
        var imageId = evt.imageId;
        
        if (imageId == -1) {
            self.selectedImage = null;
        }
        else {
            var pageNumber = getCurrentPageNumber();
            self.selectedImage = new SelectedImage(imageId, evt.imageSource, pageNumber);
        }

        self.fire(self.ImageSelectedEventName,
                        {
                            imageIsSelected: self.selectedImage != null,
                            selectedImage: self.selectedImage
                        });
    };

    this.goToSelectedImage = function () {
        if (this.selectedImage != null) {
            var currentPageNumber = getCurrentPageNumber();

            if (this.selectedImage.pageNumber != currentPageNumber) {
                this.loadAndShowImagesForPage(this.selectedImage.pageNumber);
            }
        }
    };
       
    initialize();
}

function ImageLoader(numberOfImagesToLoad, postUrl) {
    EventDispatchManager.call(this);
    var self = this;

    this.ImagesRetrievedEventName = "ImageLoader_ImagesRetrieved";
    this.ImageRetrievalFailedEventName = "ImageLoader_ImageRetrievalFailed";

    this.registerEventType(new EventType({ name: this.ImagesRetrievedEventName }));
    this.registerEventType(new EventType({ name: this.ImageRetrievalFailedEventName }));

    this.numberOfImagesToLoad = numberOfImagesToLoad;
    this.postUrl = postUrl;

    this.setImagesPerPage = function () {
        if (this.numberOfImagesToLoad === undefined) {
            this.numberOfImagesToLoad = this.imageViewer.getNumberOfImagesPerPage();
        }
    };

    this.setPostUrl = function () {
        if (this.postUrl === undefined) {
            this.postUrl = this.imageViewer.getUrlToRetrieveImages();
        }
    };

    this.loadImagesForPage = function (pageNumber) {
        this.setImagesPerPage();
        this.setPostUrl();

        var postData = {
            skip: (pageNumber - 1) * this.numberOfImagesToLoad,
            take: this.numberOfImagesToLoad
        };

        //make ajax call
        var jqxhr = $.post(this.postUrl, postData, function (data) {
            self.fire(self.ImagesRetrievedEventName, { pageNumber:pageNumber, imageList: data });
        });

        jqxhr.error(function (jqXHR, status, error) {
            self.fire(self.ImageRetrievalFailedEventName, { status: status, error: error });
        });
    };
}


function Page($pageElement) {
    var self = this;

    this.$pageElement = $pageElement;

    var rows = [];
    var numberOfImagesOnPage = -1;
    
     var initialize = function () {

        self.$pageElement.find('.ImageViewerRow').each(function (index) {
            var row = new Row($(this));
            rows.push(row);
        });

        if (rows.length == 0) {
            throw new Error("No .ImageViewerRow classes within #ImageViewerPage");
        }
    };

    this.getTotalBoxCount = function () {
        if (numberOfImagesOnPage == -1) {
            numberOfImagesOnPage = 0;

            $.each(rows, function (index, row) {
                numberOfImagesOnPage += row.getBoxCount();
            });
        }

        return numberOfImagesOnPage;
    };

    //imageWrappers is array containing ImageWrapper class
    this.updateImages = function (imageWrappers) {
        $.each(rows, function (index, row) {      
            var numberOfImagesToTake = row.getBoxCount();
            var imagesForRow = imageWrappers.splice(0, numberOfImagesToTake);
            row.updateImages(imagesForRow);
        });
    };

    this.getPageNumber = function () {
        return pageNumber;
    };

    this.registerListenerForBoxSelectedEvent = function (listener) {
        $.each(rows, function (index, row) {
            row.registerListenerForBoxSelectedEvent(listener);
        });
    };

    initialize(); 
}

function Row($rowElement) {
    var self = this;
    this.$rowElement = $rowElement;
    var boxes = [];
    
    var initialize = function () {
        //create boxes
        self.$rowElement.find('.ImageBox').each(function (index) {
            var box = new Box($(this));
            boxes.push(box);
        });
    }

    this.registerListenerForBoxSelectedEvent = function (listener) {
        $.each(boxes, function (index, box) {
            box.registerListener(box.ImageSelectedEventName, listener);
        });
    }

    this.getBoxCount = function () {
        return boxes.length;
    };

    this.updateImages = function (imageWrappers) {
        if (imageWrappers.length > boxes.length) {
            throw new Error("too many images to render");
        }
        
        $.each(boxes, function (index, box) {
            if (index < imageWrappers.length) {
                var imageWrapper = imageWrappers[index];
                box.setImage(imageWrapper);
            }
            else {
                box.hide();
            }
        });
    };    

    initialize();
}

function Box($boxElement) {
    var self = this;
    EventDispatchManager.call(this);

    this.$boxElement = $boxElement;
    this.$imageElement = $boxElement.find("img:first");
    this.isHidden = false;
    this.isMouseOver = false;
    this.isEnlarged = false;

    this.ImageSelectedEventName = "Box_ImageSelected";

    var initialise = function () {
        self.registerEventType(new EventType({ name: self.ImageSelectedEventName }));

        self.$imageElement.on("mouseenter", function () {
            self.isMouseOver = true;
            setTimeout(function () { self.enlarge(); }, 1500);
        });

        self.$imageElement.on("mouseleave", function () {
            self.isMouseOver = false;
            self.reverseEnlargement();
        });

        self.$boxElement.on("click", function () {
            var imageId = self.getImageId();
            var imageSource = self.getImageSource();

            self.fire(self.ImageSelectedEventName, { imageId: imageId, imageSource: imageSource });
        });
    };

    this.setImage = function (imageWrapper) {
        if (this.isHidden) {
            this.show();
        }

        this.$imageElement.attr("src", imageWrapper.imgSrc);
        this.$imageElement.attr("id", imageWrapper.id);
    };

    this.getImageId = function () {
        var attrVal = this.$imageElement.attr("Id");

        if (typeof attrVal === 'undefined') {
            return -1;
        }
        else {
            return attrVal;
        }
    };

    this.getImageSource = function () {
        var attrVal = this.$imageElement.attr("src");

        if (typeof attrVal === 'undefined') {
            return "#";
        }
        else {
            return attrVal;
        }
    }

    this.hide = function () {
        this.$imageElement.fadeOut(500);
        this.isHidden = true;
    };

    this.show = function () {
        this.$imageElement.fadeIn(500);
        this.isHidden = false;
    };

    this.enlarge = function () {
        if (this.isMouseOver && !this.isEnlarged) {
            var width = this.$imageElement.width();
            var height = this.$imageElement.height();

            this.$imageElement.css({
                'width': width * 2,
                'height': height * 2
            });

            //TODO: Do proper enlargement

            this.isEnlarged = true;
        }
    };

    this.reverseEnlargement = function () {
        if (this.isEnlarged) {
            var width = this.$imageElement.width();
            var height = this.$imageElement.height();

            this.$imageElement.css({
                'width': width / 2,
                'height': height / 2
            });

            this.isEnlarged = false;
        }
    };

    initialise();
}

function PagerController(initialPageNumber, numberOfImagesPerPage, leftPager, rightPager, numberPager) {
    var self = this;
    EventDispatchManager.call(this);

    this.initialPageNumber = initialPageNumber;
    this.currentPageNumber = initialPageNumber;
    this.numberOfImagesPerPage = numberOfImagesPerPage;
    this.totalNumberOfPages = 1;

    this.leftPager = leftPager;
    this.rightPager = rightPager;
    this.numberPager = numberPager;

    this.ChangePageEventName = "PagerController_ChangePage";
    this.registerEventType(new EventType({ name: this.ChangePageEventName }));

    var initialise = function () {
        registerEventHandlers();
    };

    var registerEventHandlers = function () {
        self.leftPager.registerListener(self.leftPager.ClickedEventName, function (evt) {
            self.tryChangeToPageNumber(self.currentPageNumber-1);
        });

        self.rightPager.registerListener(self.rightPager.ClickedEventName, function (evt) {
            self.tryChangeToPageNumber(self.currentPageNumber + 1);
        });

        self.numberPager.registerListener(self.numberPager.ClickedEventName, function (evt) {
            self.tryChangeToPageNumber(evt.number);
        });
    };

    this.changePage = function () {
        self.fire(self.ChangePageEventName, { pageNumber: this.currentPageNumber });
    };

    this.setCurrentPageNumber = function (pageNumber) {
        this.currentPageNumber = pageNumber;
        updatePagerElements();
    };

    this.tryChangeToPageNumber = function (pageNumber) {
        if (pageNumber > this.currentPageNumber && pageNumber <= this.totalNumberOfPages) {
            this.changeToPageNumber(pageNumber);
        }
        else if (pageNumber < this.currentPageNumber && pageNumber > 0) {
            this.changeToPageNumber(pageNumber);
        }
    };

    this.changeToPageNumber = function (newPageNumber) {
        this.currentPageNumber = newPageNumber;
        this.changePage();
    };

    this.updateForTotalNumberOfImages = function (totalNumberOfImages) {
        var newTotalNumberOfPages = totalNumberOfImages / this.numberOfImagesPerPage;
        newTotalNumberOfPages = Math.ceil(newTotalNumberOfPages);

        if (newTotalNumberOfPages > this.totalNumberOfPages) {
            this.totalNumberOfPages = newTotalNumberOfPages;
            updatePagerElements();
        }
    };

    var updatePagerElements = function () {
        var isLeftPagerEnabled = self.currentPageNumber > 0;
        self.leftPager.enableDisable(isLeftPagerEnabled);

        var isRightPagerEnabled = self.currentPageNumber < self.totalNumberOfPages;
        self.rightPager.enableDisable(isRightPagerEnabled);

        self.numberPager.update(self.totalNumberOfPages, self.currentPageNumber);
    }

    initialise();
}

function IncrementPager($pagerElement, direction) {
    EventDispatchManager.call(this);
    var self = this;

    this.$pagerElement = $pagerElement;
    this.direction = direction.toUpperCase();

    this.ClickedEventName = "IncrementPager_Clicked";
    this.registerEventType(new EventType({ name: this.ClickedEventName }));

    this.$pagerElement.on("click", function (event) {     
        if (self.isEnabled()) {
            self.fire(self.ClickedEventName, { direction: self.direction });
        }
    });

    this.isEnabled = function () {  
        return this.$pagerElement.attr('disabled') === undefined;
    };

    this.enableDisable = function (isEnabled) {
        if (isEnabled) {
            this.$pagerElement.removeAttr('disabled');
        }
        else {
            this.$pagerElement.attr('disabled', '');
        }
    };
}

function NumberedPager($pagerElement) {
    EventDispatchManager.call(this);
    var self = this;
    
    this.$pagerElement = $pagerElement;
    this.pageNumbers = [];

    this.ClickedEventName = "NumberedPager_Clicked";
    this.registerEventType(new EventType({ name: this.ClickedEventName }));

    var templatePageNumberElementHtml = null;

    this.initialise = function () {
        var $templatePageNumberElement = this.$pagerElement.find(".ImageViewerPageNumber:first");
        templatePageNumberElementHtml = $templatePageNumberElement.clone().wrap('<div>').parent().html();
        $templatePageNumberElement.remove();
    };

    this.update = function (numberOfPages, currentPageNumber) {

        if (this.pageNumbers.length != numberOfPages) {
            drawPageNumbers(numberOfPages);
        }

        this.selectPageNumber(currentPageNumber);
    };

    var drawPageNumbers = function (numberOfPages) {
        if (numberOfPages < self.pageNumbers.length) {
            while (self.pageNumbers.length > numberOfPages && self.pageNumbers.length > 0) {
                var pagerNumber = self.pageNumbers.pop();
                pagerNumber.removeFromDOM();
                pagerNumber.deRegisterListener(pagerNumber.ClickedEventName, handlePageNumberClicked);
            }

            self.pageNumbers = self.pageNumbers.splice(0, numberOfPages);
        }
        else {
            for (var i = 0; i < numberOfPages; i++) {
                if (self.pageNumbers.length <= i) {
                    var $newPageNumberElement = $(templatePageNumberElementHtml);
                    $newPageNumberElement.appendTo(self.$pagerElement);

                    var pageNumber = i + 1;

                    var pagerNumber = new NumberedPagerNumber($newPageNumberElement, pageNumber, pageNumber == 1);
                    pagerNumber.registerListener(pagerNumber.ClickedEventName, handlePageNumberClicked);
                    self.pageNumbers.push(pagerNumber);
                }
            }
        }
    };

    var handlePageNumberClicked = function (event) {
        self.fire(self.ClickedEventName, event);
    };

    var deSelectSelectedPageNumber = function () {
        for (var i = 0; i < self.pageNumbers.length; i++) {
            if (self.pageNumbers[i].isSelected()) {
                self.pageNumbers[i].deSelect();
                break;
            }
        }
    };

    this.selectPageNumber = function (pageNumber) {
        var index = pageNumber - 1;
  
        if (index >= 0 && index < this.pageNumbers.length) {
            deSelectSelectedPageNumber();
            this.pageNumbers[index].select();
        }
    };

    this.initialise();
}

function NumberedPagerNumber($numberElement, number, isSelected) {
    EventDispatchManager.call(this);
    var self = this;

    this.$numberElement = $numberElement;
    this.number = number;
    this.isSelected = isSelected;

    this.ClickedEventName = "NumberedPagerNumber_Clicked";
    this.registerEventType(new EventType({ name: this.ClickedEventName }));

    this.initialise = function () {
        var $linkElement = this.$numberElement.find('a:first');
        $linkElement.text(this.number);

        $linkElement.on("click", function (event) {
            self.fire(self.ClickedEventName, { number: self.number });
        });
    };

    this.isSelected = function(){
        return this.$numberElement.hasClass("selected"); //TODO: make selected a constant
    };

    this.select = function () {
        this.$numberElement.addClass("selected");
    };

    this.deSelect = function () {
        this.$numberElement.removeClass("selected");
    };

    this.removeFromDOM = function () {
        this.$numberElement.remove();
    };

    this.initialise();
}

function ImageWrapper(id, imgSrc) {
    this.id = id;
    this.imgSrc = imgSrc;
}

function SelectedImage(imageId, imageSource, pageNumber) {
    ImageWrapper.apply(this, arguments);
    this.pageNumber = pageNumber;
}

function EventDispatcher(eventType, eventSender) {
    this.listeners = [];
    this.eventType = eventType;
    this.eventSender = eventSender;

    this.registerListener = function (callback) {
        if (!this.containsListener(callback)) {
            this.listeners.push(callback);
        }
    };

     this.containsListener = function (callback) {
        for (var i = 0; i < this.listeners.length; i++) {
            if (this.listeners[i] === callback) {
                return true;
            }
        }

        return false;
    };

    this.deRegisterListener = function (callback) {
        for (var i = 0; i < this.listeners.length; i++) {
            if (this.listeners[i] === callback) {
                this.listeners.splice(i, 1);
                break;
            }
        }
    };

    this.clearListeners = function(){
        this.listeners = [];
    };

    this.getListenerCount = function () {
        return this.listeners.length;
    };

    this.hasListeners = function () {
        return this.listeners.length > 0;
    };

    this.fire = function (eventData) {
        var allEventData = $.extend({
            eventTypeName: this.eventType.Name,
            eventSender: this.eventSender
        },
        eventData);

        //TODO: custom logic to stop execution, handle exceptions etc
        for (var i = 0; i < this.listeners.length; i++) {
            this.listeners[i].call(this, allEventData);
        }
    };
}

function EventDispatchManager() {
    this.eventDispatchers = {};
    this.eventTypes = {};

    this.registerEventType = function (eventType) {

        var actualEventType = $.extend(new EventType(), eventType);

        if (actualEventType.Name == 'notSet') {
            throw new Error('eventType must at least have a name defined');
        }

        if (this.hasEventType(eventType.Name)) {
            throw new Error("event type with name '" + eventType.Name + "' already exists");
        }

        this.eventTypes[eventType.Name] = eventType;
        this.eventDispatchers[eventType.Name] = new EventDispatcher(eventType);
    };

    this.registerListener = function (eventTypeName, callback) {
            if (!this.hasEventType(eventTypeName)) {
            throw new Error("event type with name '" + eventTypeName + "' has not been registered");
        }

        this.eventDispatchers[eventTypeName].registerListener(callback);
    };

    this.hasEventType = function (eventTypeName) {
        return this.eventTypes.hasOwnProperty(eventTypeName);
    };

    this.deRegisterListener = function (eventTypeName, callback) {
        if (this.hasEventType(eventTypeName)) {
            this.eventDispatchers[eventTypeName].deRegisterListener(callback);
        }
    };

    this.fire = function (eventTypeName, eventData) {
        if (this.hasEventType(eventTypeName)) {
            this.eventDispatchers[eventTypeName].fire(eventData);
       }
    };
}

function EventType(opts) {
    var defaultOpts = {
        name: 'notSet'
    };

    var options = $.extend(defaultOpts, opts);

    this.Name = options.name;
}