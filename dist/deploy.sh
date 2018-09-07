#!/bin/bash

BKT="s3://webgl2.pruvalcaba.com/"
ENVI="DEVEL"
TMP_DIR="./_tmp/"

if [ "$1" = "prod" ]; then
    BKT="s3://webgl2.pruvalcaba.com/"
    ENVI="PRODUCTION"
fi
echo "Uploading DEMO to $BKT ($ENVI)"
if [ -d "$TMP_DIR" ]; then
    rm -R $TMP_DIR
fi
mkdir $TMP_DIR
cp -r demo/Build demo/TemplateData demo/index.html $TMP_DIR
aws s3 cp $TMP_DIR $BKT --recursive --acl public-read
