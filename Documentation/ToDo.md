## Introduction

This file is meant as a way to keep track of all tasks that could be executed as well as to make clear what tasks are currently the focus.

## Current Task

Currently the priority is to work on adapting the application so that it can run inside of a docker container.

### Components

* Replace System.Drawing with a linux compatible image library
  * The Bitmap in BrotImage
    * Add a dto conversion for BrotImage to create a byte array (currently this happens in the controller (!))
  * The Colors in ColorPalette (Working on this one!)
  * The Colors in the ColorPaletteDtos
  * The Colors in the ColorKernels

* Replace the json file based palette repository with sql dbs hosted in a docker container.

* Create an ImageRepository to hold data on generated images.
  * Create a docker based db for images.
  * Remove image data holding element of ImageService.
