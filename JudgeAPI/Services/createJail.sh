# $1 - submission id

#? create jail directory
mkdir jail-submission-$1
cd jail-submission-$1
mkdir -p bin lib64/x86_64-linux-gnu lib/x86_64-linux-gnu

mv ../$1-files/* ./

#? execute jail command
#sudo unshare -n -m "$2"

# mv ./test.o ../test-$1.o
# cd ..
# rm -rf ./jail-submission-$1/