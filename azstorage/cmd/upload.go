/*
Copyright © 2019 NAME HERE <EMAIL ADDRESS>

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/
package cmd

import (
	"fmt"
	"path/filepath"
	 
	"context"
 
	 
	 
	"log"
	 
	"net/url"
	"os"
 
 
	"github.com/spf13/cobra"
	"github.com/Azure/azure-storage-blob-go/azblob"
)
var Container string
var MetaDataSlice []string

func handleErrors(err error) {
	if err != nil {
		if serr, ok := err.(azblob.StorageError); ok { // This error is a Service-specific
			switch serr.ServiceCode() { // Compare serviceCode to ServiceCodeXxx constants
			case azblob.ServiceCodeContainerAlreadyExists:
				fmt.Println("Received 409. Container already exists")
				return
			}
		}
		log.Fatal(err)
	}
}

// uploadCmd represents the upload command
var uploadCmd = &cobra.Command{
	Use:   "upload [file]",
	Short: "Upload a file to Azure Blob Storage.",
	Long: `Upload a file to Azure Blob Storage.`,
	Args: cobra.MinimumNArgs(1),
	Run: func(cmd *cobra.Command, args []string) {
		ctx := context.Background() // This example uses a never-expiring context
		fmt.Println("upload called")
		// From the Azure portal, get your storage account name and key and set environment variables.
		accountName, accountKey := os.Getenv("AZURE_STORAGE_ACCOUNT"), os.Getenv("AZURE_STORAGE_ACCESS_KEY")
		if len(accountName) == 0 || len(accountKey) == 0 {
			output := "Either the AZURE_STORAGE_ACCOUNT or AZURE_STORAGE_ACCESS_KEY environment variable is not set"
			fmt.Println(output)
			log.Fatal(output)
			return
		}
		// Create a default request pipeline using your storage account name and account key.
		credential, err := azblob.NewSharedKeyCredential(accountName, accountKey)
		if err != nil {
			output := "Invalid credentials with error: " + err.Error()
			fmt.Println(output)
			log.Fatal(output)
			return
		}
		p := azblob.NewPipeline(credential, azblob.PipelineOptions{})
		// From the Azure portal, get your storage account blob service URL endpoint.
		URL, _ := url.Parse(
			fmt.Sprintf("https://%s.blob.core.windows.net/%s", accountName, Container))

		// Create a ContainerURL object that wraps the container URL and a request
		// pipeline to make requests.
		containerURL := azblob.NewContainerURL(*URL, p)
		fmt.Println(containerURL)
		path, err := os.Getwd()
		fileName := args[0]
		absoluteFilePath := filepath.Join(path, fileName)
		fmt.Println(absoluteFilePath)

		for _, v := range MetaDataSlice {
			fmt.Println(v)
		}
		
		// Here's how to upload a blob.
		blobURL := containerURL.NewBlockBlobURL(absoluteFilePath)
		file, err := os.Open(absoluteFilePath)
		handleErrors(err)

		// You can use the low-level PutBlob API to upload files. Low-level APIs are simple wrappers for the Azure Storage REST APIs.
		// Note that PutBlob can upload up to 256MB data in one shot. Details: https://docs.microsoft.com/en-us/rest/api/storageservices/put-blob
		// Following is commented out intentionally because we will instead use UploadFileToBlockBlob API to upload the blob
		// _, err = blobURL.PutBlob(ctx, file, azblob.BlobHTTPHeaders{}, azblob.Metadata{}, azblob.BlobAccessConditions{})
		// handleErrors(err)

		// The high-level API UploadFileToBlockBlob function uploads blocks in parallel for optimal performance, and can handle large files as well.
		// This function calls PutBlock/PutBlockList for files larger 256 MBs, and calls PutBlob for any file smaller
		fmt.Printf("Uploading the file with blob name: %s\n", fileName)
		_, err = azblob.UploadFileToBlockBlob(ctx, file, blobURL, azblob.UploadToBlockBlobOptions{
			BlockSize:   4 * 1024 * 1024,
			Parallelism: 16,
			Metadata:    azblob.Metadata{"a": "b"}})
		handleErrors(err)

	},
}

func init() {
	rootCmd.AddCommand(uploadCmd)

	// Here you will define your flags and configuration settings.

	// Cobra supports Persistent Flags which will work for this command
	// and all subcommands, e.g.:
	// uploadCmd.PersistentFlags().String("foo", "", "A help for foo")

	// Cobra supports local flags which will only run when this command
	// is called directly, e.g.:
	// uploadCmd.Flags().BoolP("toggle", "t", false, "Help message for toggle")
	uploadCmd.Flags().StringVarP(&Container, "container", "c", "", "container name(required)")
	uploadCmd.MarkFlagRequired("container")
	uploadCmd.Flags().StringArrayVarP(&MetaDataSlice, "metaData", "m", []string{}, "")

}
