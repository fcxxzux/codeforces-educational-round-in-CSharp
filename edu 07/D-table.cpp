#include <stdio.h>
#include <ctype.h>
#include <string.h>
#include <stdlib.h>
#include <limits.h>
#include <math.h>
#include <algorithm>
using namespace std;
typedef long long ll;

const int n=7;
int d[2*n];
int best[2*n];

int main(){
	for(int i=1;i<=n;i++){
		d[i*2-2]=d[i*2-1]=i;
	}
	int minans=INT_MAX;
	do{
		if(d[0]!=1)break;
		if(d[n-2]!=3||d[1]!=3)continue;
		int ans=0;
		for(int i=1;i<=n;i++){
			int px=-1,py=-1;
			for(int j=0;j<2*n;j++){
				if(d[j]==i){
					if(px<0)px=j;
					else{
						py=j;break;
					}
				}
			}
			ans+=(n-i)*abs(py-px+i-n);
			if(ans>minans)break;
		}
		if(ans==0){
			for(int i=0;i<2*n;i++){
				printf("%d ",d[i]);
			}
			printf("%d\n",ans);
		}
		
		if(ans<minans){
			for(int i=0;i<2*n;i++){
				best[i]=d[i];
			}
			minans=ans;
		}
	}while(next_permutation(d,d+2*n));
	printf("minans=%d\n",minans);
	for(int i=0;i<2*n;i++){
		printf("%d ",best[i]);
	}
    return 0;
}

